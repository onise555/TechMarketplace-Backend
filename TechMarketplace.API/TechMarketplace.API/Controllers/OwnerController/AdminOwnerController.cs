using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminUserDtos;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Requests.OwnerRequest;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.OwnerController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="SuperAdmin")]
    public class AdminOwnerController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public AdminOwnerController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }


        [HttpPost("Create-Admin")]
        public ActionResult CreateAdmin(CreateAdminRequest req)
        {
       
         
                if (_data.Users.Any(u => u.Email == req.Email))
                {
                    return BadRequest("Email already in use");
                }

                var admin = new Models.Users.User
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                    Role = UserRole.Admin,
                    IsVerified = true,
                    IsActive = true
                };

                _data.Users.Add(admin);
                _data.SaveChanges();

            _emailSender.SendMail(admin.Email, "Admin Account Created",
              $"Hello {admin.FirstName}, your Admin account has been created by SuperAdmin.");


            return Ok(new
                {
                    Message = "Admin created successfully",
                    AdminId = admin.Id,
                    admin.Email
                });
            }



        [HttpGet("All-Users")]
        public ActionResult GetAllUsers()
        {
            var users = _data.Users.Select(u => new UserDtos
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsVerified = u.IsVerified,
                IsActive = u.IsActive
            }).ToList();

            return Ok(users);
        }


        [HttpDelete("Delete-User/{id}")]
        public ActionResult DeleteUser(int id)
        {
      
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim) || !int.TryParse(currentUserIdClaim, out int currentUserId))
            {
                return Unauthorized("Invalid token");
            }

         
            if (currentUserId == id)
            {
                return Forbid("SuperAdmin-ს არ შეუძლია საკუთარი ანგარიშის წაშლა");
            }

            var user = _data.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return BadRequest("User not found");
            }

        
            _data.Users.Remove(user);
            _data.SaveChanges();

         
                _emailSender.SendMail(user.Email, "Account Deleted",
              $"Hello {user.FirstName}, your account has been permanently deleted by SuperAdmin.");
            
           

            return Ok(new
            {
                Message = "User permanently deleted",
                DeletedUserId = id
            });
        }



        [HttpGet("System-Info")]
        public ActionResult GetSystemInfo()
        {
            var totalUsers = _data.Users.Count();
            var activeUsers = _data.Users.Count(u => u.IsActive);
            var adminCount = _data.Users.Count(u => u.Role == UserRole.Admin);
            var managerCount=_data.Users.Count(u=> u.Role == UserRole.Manager);

            return Ok(new
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                AdminCount = adminCount,
                ManagerCount=managerCount,
                SystemDate = DateTime.Now

            });
        }

        [HttpGet("Get-CategorY-info")]
        public ActionResult GetCategory()
        {
            var categorycount = _data.Categories.Count();
            var totalSubCategory = _data.SubCategories.Count();
            var totalProducts = _data.Products.Count();

            return Ok(new
            {
                TotaAlCategory = categorycount,
                TotalSubCategory = totalSubCategory,
                TotalProducts = totalProducts,
                SystemDate = DateTime.Now

            });
        }

    }
}