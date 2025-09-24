using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.User.WishListDtos;
using TechMarketplace.API.Models.WishLists;
using TechMarketplace.API.Requests.User.WishLisRequests;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWishLisController : ControllerBase
    {   
        private readonly DataContext _data;

        public UserWishLisController(DataContext data)
        {
            _data = data;   
        }

        [Authorize(Roles = "User")]
        [HttpPost("User-WishList")]
        public ActionResult AddWishlist(CreateWishListRequest request)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (request.UserId != currentUserId)
            {
                return Forbid();
            }

            var wishlist = new WishList()
            {
                UserId = currentUserId,
                Name = request.Name,
            };

            _data.WishLists.Add(wishlist);
            _data.SaveChanges();

            return Ok(wishlist);
        }


        [HttpGet("wishlists")]
        public ActionResult GetWishlist()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var wishlist = _data.WishLists.Where(x => x.UserId == currentUserId).Select(x => new GetWishListDtos
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return Ok(wishlist);
        }

        [Authorize(Roles = "User")]
        [HttpPut("Update-WishList/{id}")]
        public ActionResult UpdateWishList(int id, UpdateWishListRequest updateRequest)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var wishlist = _data.WishLists.FirstOrDefault(x => x.Id == id && x.UserId == currentUserId);
            if (wishlist == null)
            {
                return NotFound("WishList Not Found");
            }

            wishlist.Name = updateRequest.Name; 

            _data.SaveChanges();

            return Ok(new { Id = wishlist.Id, Name = wishlist.Name });
        }

        [Authorize(Roles = "User")]
        [HttpDelete("WishList/{id}")]

        public ActionResult DeleteWishList(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var wishlist =_data.WishLists.FirstOrDefault(x=>x.Id==id);

            if (wishlist == null)
            {
                return NotFound("WishList Not Founded ");
            }


            _data.WishLists.Remove(wishlist);
            _data.SaveChanges();


            var deletedto = new DeleteWishListDtos
            {
                Id = id,
            };
             return Ok(deletedto);

        }
    }
}
