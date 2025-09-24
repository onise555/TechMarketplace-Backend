using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Product;
using TechMarketplace.API.Dtos.User;
using TechMarketplace.API.Dtos.User.CartDtos;
using TechMarketplace.API.Dtos.User.CartItemDtos;
using TechMarketplace.API.Models.Carts;
using TechMarketplace.API.Requests.User.CartRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserCartController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }


        [Authorize(Roles = "User")]
        [HttpPost("Cart-User")]
        public ActionResult AddCart(CreateCartRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (req.UserId != currentUserId)
            {
                return Forbid();
            }

            var existingCart = _data.Carts.FirstOrDefault(x => x.UserId == currentUserId);
            if (existingCart != null)
            {
                return BadRequest("User already has a cart");
            }

            Cart cart = new Cart()
            {
                CreateAt = DateTime.UtcNow,
                UserId = currentUserId,
            };

            _data.Carts.Add(cart);
            _data.SaveChanges();

            return Ok(cart);


        }




        [Authorize(Roles = "User")]
        [HttpGet("Cart/{userid}/items")]
        public ActionResult GetAction(int userid)
        {
            var currentid = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userid != currentid)
                return Forbid();

            var cart = _data.Carts
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.UserId == userid);

            if (cart == null)
                return NotFound("Cart Not found");

            var cartDto = new UserCartDtos
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                CreatedAt = cart.CreateAt,
                Items = cart.Items
                    .Where(i => i.Product != null) 
                    .Select(item => new CartItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        cartItemProductDtos = new CartItemProductDtos
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                            ProductImgUrl = item.Product.ProductImgUrl,
                            Price = item.Product.Price
                        },
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    }).ToList(),
                TotalItems = cart.Items.Sum(x => x.Quantity),
                TotalAmount = cart.Items.Sum(x => x.TotalPrice)
            };

            return Ok(cartDto);
        }


        [Authorize(Roles = "User")]
        [HttpDelete("{userId}/clear")]
        public ActionResult ClearCart(int userId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Security check
            if (currentUserId != userId)
            {
                return Forbid("Cannot clear another user's cart");
            }

            // Find user's cart first
            var cart = _data.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return NotFound("Cart not found");
            }

            // Get cart items
            var cartItems = _data.CartItems.Where(x => x.CartId == cart.Id).ToList();

            if (cartItems.Any())
            {
                var removedIds = cartItems.Select(x => x.Id).ToList();

                _data.CartItems.RemoveRange(cartItems);
                _data.SaveChanges();

                return Ok(new 
                {
                    RemovedItemIds = removedIds
                });
            }

            return Ok(new CartDeleteDtos());
        }

    }
}