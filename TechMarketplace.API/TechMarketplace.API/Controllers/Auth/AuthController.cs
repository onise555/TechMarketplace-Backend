using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechMarketplace.API.Data;
using TechMarketplace.API.Services;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Models.Users;
using UserEntity = TechMarketplace.API.Models.Users.User;
using TechMarketplace.API.SMTP;
using TechMarketplace.API.Requests.User.AuthRequests;
namespace TechMarketplace.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public AuthController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
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


            UserEntity user = new UserEntity
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
        [HttpPost("Resend-Code")]
        public ActionResult ResendCode([FromBody] ResendCodeRequest req)
        {
            var user = _data.Users.FirstOrDefault(x => x.Email == req.Email);
            if (user == null)
                return NotFound("User not found");

            if (user.IsVerified)
                return BadRequest("User already verified");

            Random random = new Random();
            var code = random.Next(100000, 1000000).ToString();

            var NewCode = user.VerifyCode = code;
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

            if (!user.IsVerified)
                return BadRequest("Please verify your email before logging in.");



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
            var user = _data.Users.FirstOrDefault(u => u.Email == req.Email);
            if (user == null)
                return BadRequest("User Not Founded");

            if (!user.IsVerified)
                return BadRequest("User email is not verified");

            var code = new Random().Next(100000, 1000000).ToString();
            user.VerifyCode = code;
            user.VerifyCodeExpiresAt = DateTime.UtcNow.AddMinutes(10);

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Password Reset Code", $"Your reset code is: <b>{code}</b>");

            return Ok(new { message = "Password reset code sent to your email." });

        }


        [HttpPost("Reset-Password")]
        public ActionResult ResetPassword([FromBody] ResetPasswordRequest req)
        {

            var user = _data.Users.FirstOrDefault(u => u.VerifyCode == req.Code);

            if (user == null)
                return BadRequest(new { error = "Invalid reset code" });

            if (DateTime.UtcNow > user.VerifyCodeExpiresAt)
                return BadRequest(new { error = "Reset code expired" });

            user.Password = BCrypt.Net.BCrypt.HashPassword(req.NewPassword);
            user.VerifyCode = null;
            user.VerifyCodeExpiresAt = null;

            _data.SaveChanges();

            _emailSender.SendMail(user.Email, "Password Changed", "Your password has been successfully reset.");

            return Ok(new { message = "Password has been reset successfully." });
        }

        #endregion


    }
}
