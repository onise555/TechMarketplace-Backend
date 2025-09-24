using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminUserDtos;
using TechMarketplace.API.Dtos.User.ProfileDtos;
using TechMarketplace.API.Requests.User.AuthRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserProfileController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }




        #region Profile
        [Authorize(Roles = "User")]
        [HttpGet("Get-User/{id}")]
        public ActionResult GetUser(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != currentUserId)
                return Forbid("You can only access your own profile");

            // მხოლოდ User role-ს ვეძებთ
            var user = _data.Users.FirstOrDefault(x => x.Id == id && x.Role == UserRole.User);

            if (user == null)
                return NotFound("User not found or not a regular user");

            var userDto = new UserDtos
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                IsVerified = user.IsVerified,
                IsActive = user.IsActive,
            };

            return Ok(userDto);
        }

        [Authorize(Roles = "User")]
        [HttpPut("Update-User-Profile/{id}")]
        public ActionResult UpdateProfile(int id, UpdateUserRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (id != currentUserId)
                return Forbid();

            var user = _data.Users.FirstOrDefault(x => x.Id == id);

            if (_data.Users.Any(u => u.Email == req.Email && u.Id != id))
                return BadRequest("Email is already in use.");

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Email = req.Email;
            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Change Password", "Password");
            var UserDto = new UpdateUserDtos
            {
                Id = user.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,

            };

            return Ok(UserDto);
        }
        #endregion


        #region AccountStatus
        [HttpPost("Deactivate-User")]
        public ActionResult DeactiveUser()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = _data.Users.FirstOrDefault(u => u.Id == currentUserId);
            if (user == null) return
                    BadRequest("User Not Founded");

            if (!user.IsActive)
                return BadRequest("User is already deactivated");

            user.IsActive = false;
            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Account Deactivated", $"Hello {user.FirstName}, your account has been deactivated");

            return Ok(new
            {
                Message = "Your account has been deactivated successfully",
                user.Id,
                user.Email
            });
        }

        // Delete User
        [Authorize(Roles = "User")]
        [HttpDelete("Delete-User/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != currentUserId)
                return Forbid();

            var user = _data.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound("User Not Founded");

            _data.Users.Remove(user);
            _data.SaveChanges();
            _emailSender.SendMail(user.Email, "Account Deleted", $"Hello {user.FirstName}, your account has been deleted");

            var userDto = new DeleteUserDtos
            {
                Id = user.Id,
            };
            return Ok(userDto);
        }
        #endregion



    }
}
