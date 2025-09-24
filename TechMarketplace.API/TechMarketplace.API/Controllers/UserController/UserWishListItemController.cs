using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Product;
using TechMarketplace.API.Dtos.User.WishListItemDtos;
using TechMarketplace.API.Models.WishLists;
using TechMarketplace.API.Requests.User.WishListItemRequests;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWishListItemController : ControllerBase
    {

        private readonly DataContext _data;

        public UserWishListItemController(DataContext data)
        {
            _data = data;
        }

        [HttpPost("Add-Item")]
        public ActionResult AddWishListItem(CreateWishListItemRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            var existingItem = _data.WishListItems
                .FirstOrDefault(w => w.WishListId == req.WishListId && w.ProductId == req.ProductId);

            if (existingItem != null)
            {

                existingItem.Quantity++;
                _data.SaveChanges();

                return Ok(new
                {
            
                    item = existingItem
                });
            }

            WishListItem wishlistitem = new WishListItem()
            {
                WishListId = req.WishListId,
                ProductId = req.ProductId,
            };


            _data.WishListItems.Add(wishlistitem);
            _data.SaveChanges();

            return Ok(new
            {
            item = wishlistitem
            });
        }




        [HttpGet("Get-WishList-Items/{wishListId}")]
        public ActionResult GetWishListItems(int wishListId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (currentUserId == 0)
                return Unauthorized("User is not authorized");

            var wishList = _data.WishLists
                .FirstOrDefault(w => w.Id == wishListId && w.UserId == currentUserId);

            if (wishList == null)
                return NotFound("WishList not found or does not belong to you");

           
            var items = _data.WishListItems
                .Where(w => w.WishListId == wishListId)
                .Include(w => w.Product)
                .Select(w => new GetWishListItemDtos
                {
                    Id = w.Id,
                    WishListId = w.WishListId,
                    ProductId = w.ProductId,
                    Quantity = w.Quantity,
                    CreatedAt = w.CreatedAt,
                    Product = new ProductDtos
                    {
                       
                        Id = w.Product.Id,
                        Name = w.Product.Name,
                        Price = w.Product.Price,
                
                    }
                })
                .ToList();

            return Ok(items);
        }

        [HttpGet("Get-Item/{itemId}")]
        public ActionResult GetWishListItem(int itemId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (currentUserId == 0)
                return Unauthorized("User is not authorized");

         
            var item = _data.WishListItems
                .Include(w => w.Product)
                .Include(w => w.WishList)
                .Where(w => w.Id == itemId && w.WishList.UserId == currentUserId)
                .Select(w => new GetWishListItemDtos
                {
                    Id = w.Id,
                    WishListId = w.WishListId,
                    ProductId = w.ProductId,
                    Quantity = w.Quantity,
                    CreatedAt = w.CreatedAt,
                    Product = new ProductDtos
                    {
                        Id = w.Product.Id,
                        Name = w.Product.Name,
                        Price = w.Product.Price,

                    }
                })
                .FirstOrDefault();

            if (item == null)
                return NotFound("WishListItem not found or does not belong to you");

            return Ok(item);
        }


        [Authorize(Roles = "User")]
        [HttpDelete("Delete-Item/{itemId}")]
        public ActionResult DeleteWishListItem(int itemId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var item = _data.WishListItems
                .Include(w => w.WishList)
                .FirstOrDefault(w => w.Id == itemId && w.WishList.UserId == currentUserId);

            if (item == null)
            {
                return NotFound("WishListItem not found or does not belong to you");
            }

    
            _data.WishListItems.Remove(item);
            _data.SaveChanges();

            return Ok(new
            {
                message = "Product removed from WishList",
                deletedItemId = itemId
            });
        }
    }
}