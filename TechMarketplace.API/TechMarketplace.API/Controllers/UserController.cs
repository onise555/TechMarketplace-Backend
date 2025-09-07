using BCrypt;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.User;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Requests.User;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }


        #region RegistrationVerification 

        // Registers a new user 

        [HttpPost("Registration")]
        public ActionResult Registration([FromBody] CreateUserRequest req)
        {
            var emailNormalized = req.Email.Trim().ToLowerInvariant();

        
            if (_data.Users.Any(u => u.Email.ToLower() == emailNormalized))
                return BadRequest("Email is already in use");

        
            var code = new Random().Next(100000, 1000000).ToString();

         
            User user = new User()
            {
                FirstName = req.FirstName.Trim(),
                LastName = req.LastName.Trim(),
                Role = UserRole.User, 
                Email = emailNormalized, 
                Password = BCrypt.Net.BCrypt.HashPassword(req.Password),
                VerifyCode = code,
                VerifyCodeExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false,
                IsActive = true
            };

            _data.Users.Add(user);
            _data.SaveChanges();

            // Send verification email
            _emailSender.SendMail(emailNormalized, "Verification Code", $"Your code is: <b>{code}</b>");

            return Ok("Registration successful. Check your email for verification code.");
        }



       // Verifies a user

        [HttpPost("Verify")]

        public ActionResult Verify([FromBody] VerifyRequest req)
        {
            var user = _data.Users.FirstOrDefault(x => x.Email == req.Email);

            if (user == null)
              return BadRequest(" User Not Founder");

            if (DateTime.UtcNow > user.VerifyCodeExpiresAt)
                return BadRequest("time is end try agine");

            if (user.VerifyCode == req.Code)
            {
                user.IsVerified = true;
                user.VerifyCode = null;
                _data.SaveChanges();
                return Ok("Email verified successfully!");
            }
            return BadRequest("This Email Not Founded");

        }

        // Resends the email verification code 
        [HttpPost( "Resend-Code")]
        public ActionResult ResendCode([FromBody] ResendCodeRequest req)
        {
            var user =_data.Users.FirstOrDefault(x=>x.Email == req.Email);
            if (user == null)
                return NotFound("User not found");

            if (user.IsVerified)
                return BadRequest("User already verified");                     

            Random random = new Random();
            var code = random.Next(100000, 1000000).ToString();

           var NewCode= user.VerifyCode = code;
           user.VerifyCodeExpiresAt = DateTime.UtcNow.AddMinutes(1);

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Verification Code", $"Your new code is: <b>{NewCode}</b>");

            return Ok("New verification code sent successfully.");

        }
      
        // Logs in the user and returns JWT token
        [HttpPost("Login")]
        public ActionResult Login([FromBody] CreateLoginRequest req)
        {
            var user = _data.Users.FirstOrDefault(x => x.Email == req.Email);
            if (user == null)
                return BadRequest("User not found");

            var isValid = BCrypt.Net.BCrypt.Verify(req.Password, user.Password);
            if (!isValid)
                return BadRequest("Invalid password");

            var token = _Jwt.GenerateToken(user.Id, user.FirstName, new List<UserRole> { user.Role });


            return Ok(new
            {
                Message = "Login successful",
                Token = token,
                User = new
                {
                    user.Id,
                    user.FirstName,
                    user.Email,
                    Role = user.Role.ToString()
                }
            });
        }
        #endregion


        #region PasswordManagement
        [HttpPost("Forgot-Password")]
        public ActionResult ForgetPassword([FromBody] ForgotPasswordRequest req)
        {
            var user=_data.Users.FirstOrDefault(u=>u.Email == req.Email);
            if (user == null)
                return BadRequest("User Not Founded");

            if (!user.IsVerified)
                return BadRequest("User email is not verified");

            var code = new Random().Next(100000, 1000000).ToString();
            user.VerifyCode = code;
            user.VerifyCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Password Reset Code", $"Your reset code is: <b>{code}</b>");

            return Ok("Password reset code sent to your email.");

        }

        [HttpPost("Reset-Password")]
        public ActionResult ResetPassword([FromBody] ResetPasswordRequest req)
        {
            var user = _data.Users.FirstOrDefault(u=>u.Email == req.Email);

            if (user == null) 
                return BadRequest("User Not Founded");

            if (DateTime.UtcNow > user.VerifyCodeExpiresAt)
                return BadRequest("Reset code expired");

            if (user.VerifyCode != req.Code)
                return BadRequest("Invalid reset code");

            user.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.VerifyCode = null;
            user.VerifyCodeExpiresAt=null;

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Password Changed", "Your password has been successfully reset.");

            return Ok("Password has been reset successfully.");
        }
        #endregion


        #region Profile
        [Authorize(Roles ="User")]
        [HttpGet("Get-User/{id}")]
        public ActionResult GetUser(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != currentUserId)
                return Forbid("You can only access your own profile");


            var user =_data.Users.FirstOrDefault(x=>x.Id == id);

           
            if (user == null)
                return NotFound("User not found");

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

        [Authorize(Roles ="User")]
        [HttpPut("Update-User-Profile/{id}")]
        public ActionResult UpdateProfile(int id ,UpdateUserRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (id != currentUserId)
                return Forbid();

            var user = _data.Users.FirstOrDefault(x=>x.Id == id);

            if (_data.Users.Any(u => u.Email == req.Email && u.Id != id))
                return BadRequest("Email is already in use.");

           user.FirstName = req.FirstName;
           user.LastName = req.LastName;
           user.Email = req.Email;
           user.Password=BCrypt.Net.BCrypt.HashPassword(req.Password);
            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Change Password", "Password");
            var UserDto = new UpdateUserDtos
            {
                Id = user.Id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email,

            };

            return  Ok(UserDto);
        }
        #endregion


        #region AccountStatus
        [HttpPost("Deactive-User")]
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

            if(id != currentUserId)
                return Forbid();

            var user = _data.Users.FirstOrDefault(x=>x.Id==id);

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
