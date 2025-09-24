using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Product;
using TechMarketplace.API.Dtos.User.OrderDtos;
using TechMarketplace.API.Models.Orders;
using TechMarketplace.API.Requests.User.OrderRequests;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOrderController : ControllerBase
    {
        private readonly DataContext _data;
        public UserOrderController(DataContext data)
        {
            _data = data;
        }

        [HttpPost("Create-Order")]
        public ActionResult CreateOrder(CreateOrderRequest request)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Address ownership შემოწმება
            var address = _data.Addresses
                .FirstOrDefault(a => a.Id == request.AddressId && a.UserId == currentUserId);

            if (address == null)
                return BadRequest("Address not found or does not belong to you");

            // Cart მიღება
            var cart = _data.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == currentUserId);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Cart is empty");

           
            var order = new Order
            {
                UserId = currentUserId,
                AddressId = request.AddressId, 
                TotalAmount = cart.Items.Sum(item => item.Product.Price * item.Quantity),
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow
            };

            _data.Orders.Add(order);
            _data.SaveChanges();

         
            foreach (var cartItem in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                };
                _data.OrderItems.Add(orderItem);
            }

      
            _data.CartItems.RemoveRange(cart.Items);
            _data.SaveChanges();

            return Ok(new
            {
                message = "Order created successfully",
                orderId = order.Id
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("My-Orders")]
        public ActionResult GetMyOrders()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var orders = _data.Orders
                .Where(o => o.UserId == currentUserId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    OrderDate = o.OrderDate,
                    DeliveryDate = o.DeliveryDate,
                    ItemCount = o.Items.Count()
                })
                .ToList();

            return Ok(orders);
        }



        [Authorize(Roles = "User")]
        [HttpGet("Order/{id}")]
        public ActionResult GetOrder(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var order = _data.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Address)
                .FirstOrDefault(o => o.Id == id && o.UserId == currentUserId);

            if (order == null)
                return NotFound("Order not found");

            var orderDetails = new OrderDetailsDtos
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Address = new OrderAddressDtos
                {
                    Id = order.Address.Id,
                    Street = order.Address.Street,
                    City = order.Address.City,
                    Country = order.Address.Country,
                    PostalCode = order.Address.ZipCode
                },
                Items = order.Items.Select(oi => new OrderItemDtos
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    ProductDtos = new ProductDtos
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Price = oi.Product.Price,
                        CreatedAt = oi.Product.CreatedAt,
                        Model= oi.Product.Model,    
                        ProductImgUrl = oi.Product.ProductImgUrl,
                        Sku= oi.Product.Sku,    
                        Status = oi.Product.Status, 
                        
                    }
                }).ToList()
            };

            return Ok(orderDetails);
        }
        [Authorize(Roles = "User")]
        [HttpPut("Cancel-Order/{id}")]
        public ActionResult CancelOrder(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var order = _data.Orders
                .FirstOrDefault(o => o.Id == id && o.UserId == currentUserId);

            if (order == null)
                return NotFound("Order not found");

            if (order.Status != OrderStatus.Pending)
                return BadRequest("Cannot cancel order with current status");

            order.Status = OrderStatus.Cancelled;
            _data.SaveChanges();

            return Ok(new { message = "Order cancelled successfully" });
        }


        [HttpPost("Buy-Now")]
        public ActionResult BuyNow(BuyNowRequest request)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

       
            var address = _data.Addresses
                .FirstOrDefault(a => a.Id == request.AddressId && a.UserId == currentUserId);

            if (address == null)
                return BadRequest("Address not found or does not belong to you");

            var product = _data.Products
                .FirstOrDefault(p => p.Id == request.ProductId);

            if (product == null)
                return BadRequest("Product not found");


            var productDetail = _data.ProductDetails
                .FirstOrDefault(pd => pd.ProductId == request.ProductId);

            if (productDetail != null && productDetail.Stock < request.Quantity)
                return BadRequest("Insufficient stock");

  
            var order = new Order
            {
                UserId = currentUserId,
                AddressId = request.AddressId,
                TotalAmount = product.Price * request.Quantity,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow
            };

            _data.Orders.Add(order);
            _data.SaveChanges();

      
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = product.Price
            };

            _data.OrderItems.Add(orderItem);

            if (productDetail != null)
            {
                productDetail.Stock -= request.Quantity;
            }

            _data.SaveChanges();

            return Ok(new
            {
                message = "Order created successfully via Buy Now",
                orderId = order.Id,
                totalAmount = order.TotalAmount
            });
        }

    }
}
