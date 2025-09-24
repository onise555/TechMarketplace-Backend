using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.User;
using TechMarketplace.API.Dtos.User.CartItemDtos;
using TechMarketplace.API.Models.Carts;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.User.CartItemRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartItemController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserCartItemController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "User")]
        [HttpPost("Add-Item")]
        public ActionResult AddItem(CreateCartItemRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var cart = _data.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.Id == req.CartId && c.UserId == currentUserId);

            if (cart == null)
                return NotFound("Cart not found or does not belong to current user");

            var product = _data.Products.FirstOrDefault(p => p.Id == req.ProductId);
            if (product == null)
                return NotFound("Product not found");

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                CartId = cart.Id,
                Price = product.Price,
                Quantity = req.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            _data.CartItems.Add(cartItem);
            _data.SaveChanges();

            return Ok(new
            {
                cartItem.Id,
                cartItem.ProductId, 
                ProductName = product.Name,
                ProductImage = product.ProductImgUrl,
                cartItem.Price,
                cartItem.Quantity,
                cartItem.TotalPrice
            });
        }

        [Authorize(Roles = "User")]
        [HttpPut("Item/{id}/Quantity")]
        public ActionResult UpdateQuantity(int id, UpdateCartitemQuantityRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

    
            var cartItem = _data.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Product) 
                .FirstOrDefault(ci => ci.Id == id && ci.Cart.UserId == currentUserId);

            if (cartItem == null)
            {
                return NotFound("Cart Item Not Found");
            }

            if (req.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            cartItem.Quantity = req.Quantity;
            _data.SaveChanges();

            return Ok(cartItem.Quantity);
        }


        [Authorize(Roles = "User")]
        [HttpDelete("Delete-Cat-Item/{id}")]
        public ActionResult CartItem(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);



            var cartItem = _data.CartItems
           .Include(ci => ci.Cart)
           .Include(ci => ci.Product)
           .FirstOrDefault(ci => ci.Id == id && ci.Cart.UserId == currentUserId);
            if (cartItem == null)
            {
                return NotFound("Product Not Founded");
            }
            
            
            _data.CartItems.Remove(cartItem);
            _data.SaveChanges();

            var deleteItremDto = new DeleteCartItem
            {
                Id = id,
            };

            return Ok(deleteItremDto);

        }

    }
}
