using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminUserDtos;
using TechMarketplace.API.Requests.Admin.AdminUserRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public AdminController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt=jwtServices;
            _emailSender=emailSender;

        }

        #region User Management
        [HttpGet("Users")]
        public ActionResult GetAllUser()
        {

            var user = _data.Users
                .Where(u=>u.IsActive && u.Role!=UserRole.Admin)
                .Select(x => new UserDtos
                {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Role = x.Role,
                IsVerified = x.IsVerified,
                IsActive = x.IsActive,

            }).ToList();

            if (user == null) return
                BadRequest("User Not Founded");

            return Ok(user);
        }

        [HttpGet("Get-User/{id}")]
        public ActionResult GetUser(int id)
        {
            var user = _data.Users.Where(x=>x.Id == id)
                .Select(x=> new UserDtos 
                { 
                Id=x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Role = x.Role,
                IsVerified = x.IsVerified,
                IsActive = x.IsActive,

                })
                .FirstOrDefault();

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

       
        [HttpPost("Create-Manager")]
        public ActionResult CreateManager(CreateManagerRequest req)
        {


            var emailNormalized = req.Email.Trim().ToLowerInvariant();
            if ( _data.Users.Any(u => u.Email.ToLower() == emailNormalized))
                return BadRequest("Email already in use");
      


            var user = new Models.Users.User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = emailNormalized,
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                Role = UserRole.Manager,
                IsVerified = true,
                IsActive = true
                
            };

            _data.Users.Add(user);
            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Verification Code", $"create Maneger <b>{user.FirstName}</b>");

            return Ok(new { Message = "Manager created successfully", emailNormalized = user.Email });
        }


        [HttpPut("Update-User/Role/{id}")]
        public ActionResult UpdateRole(int id, UpdateUserRoleRequest req)
        {
   
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(currentUserIdClaim, out int currentUserId))
                return Unauthorized("Invalid token");

        
            var currentUser = _data.Users.FirstOrDefault(u => u.Id == currentUserId);
            var targetUser = _data.Users.FirstOrDefault(u => u.Id == id);

            if (targetUser == null)
                return BadRequest("User not found");

            
            if (!Enum.IsDefined(typeof(UserRole), req.Role))
                return BadRequest("Invalid role");

            if (currentUserId == id)
                return Forbid("you can not change your role");

            if (currentUser.Role == UserRole.Admin && targetUser.Role == UserRole.Admin)
                return Forbid("you can not cange adin role");

            targetUser.Role = req.Role;
            _data.SaveChanges();

            return Ok(new { Message = "Role updated successfully" });
        }


        [HttpPut("Update-User/{id}")]
        public ActionResult UpdateUser(int id , UpdateUser req)
        {
            var user = _data.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return BadRequest("User Not Founded");

            if (_data.Users.Any(u => u.Email == req.Email && u.Id != id))
                return BadRequest("Email already in use");

            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Email = req.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
            user.Role = req.Role;   

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Update", $"Hello {user.FirstName}, your account details have been updated by an administrator.");

            return Ok(new { Message = "User updated successfully" });
        }

        [HttpPost("Activate-User/{id}")]
        public ActionResult ActivateUser(int id)
        {
            var user = _data.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return BadRequest("User not found");

        
            if (user.Role == UserRole.Admin || user.Role == UserRole.SuperAdmin)
                return Forbid("Cannot activate admin users");

          
            if (user.IsActive && user.IsVerified)
                return BadRequest("User is already active and verified");

            user.IsActive = true;
            user.IsVerified = true;
            _data.SaveChanges();

        
          
                _emailSender.SendMail(user.Email, "Account Activated",
                    $"Hello {user.FirstName}, your account has been activated by an administrator.");
        
      
            return Ok(new { Message = "User activated successfully" });
        }


        #endregion


        [HttpGet("all-payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _data.Payments
                    .Select(p => new
                    {
                        paymentId = p.Id,
                        orderId = p.OrderId,
                        amount = p.Amount,
                        status = p.Status.ToString(),
                        transactionId = p.TransactionId,
                        paymentMethod = p.PaymentMethod,
                        createdAt = p.CreatedAt,
                        paidAt = p.PaidAt
                    })
                    .ToListAsync();

                return Ok(new { payments = payments, count = payments.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
