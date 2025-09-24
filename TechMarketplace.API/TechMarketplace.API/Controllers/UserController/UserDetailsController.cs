using BCrypt;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.User;
using TechMarketplace.API.Dtos.User.ProfileDtos;
using TechMarketplace.API.Dtos.User.UserDetailDtos;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Requests.User.UserDetailRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;
using TechMarketplace.API.Validators.User;

namespace TechMarketplace.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserDetailsController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }



        #region User Details

        [Authorize(Roles = "User")]
        [HttpPost("User-Details")]
        public ActionResult UserDetail(CreateUserDetailRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (req.UserId != currentUserId)
                return Forbid();



            UserDetail detail = new UserDetail()
            {
                UserProfileImg = req.UserProfileImg,
                DateOfBirth = req.DateOfBirth,
                PhoneNumber = req.PhoneNumber,
                UserId = req.UserId,
            };
            _data.UserDetails.Add(detail);
            _data.SaveChanges();
            return Ok(detail);

        }


        [Authorize(Roles = "User")]
        [HttpGet("User-Details/{id}")]
        public ActionResult GetUserDetail(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);



            var UserDetail = _data.UserDetails
                .Where(x => x.UserId == id)
                .Select(x => new UserDetailDtos
                {
                    Id = x.Id,
                    UserProfileImg = x.UserProfileImg,
                    DateOfBirth = x.DateOfBirth,
                    PhoneNumber = x.PhoneNumber,

                }).FirstOrDefault();

            return Ok(UserDetail);
        }

        [Authorize(Roles = "User")]
        [HttpPut("Update-User-Details/{id}")]
        public ActionResult UpdateUserDetail(int id, UpdateUserDetailRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (currentUserId != id)
                return Forbid();

            var userDetail = _data.UserDetails.FirstOrDefault(x => x.UserId == id);
            if (userDetail == null)
                return BadRequest("User Detail not found");

            userDetail.UserProfileImg = req.UserProfileImg;
            userDetail.PhoneNumber = req.PhoneNumber;
            userDetail.DateOfBirth = req.DateOfBirth;

            _data.SaveChanges();

            var userDetailDto = new UserDetailDtos
            {
                Id = userDetail.Id,
                UserProfileImg = userDetail.UserProfileImg,
                DateOfBirth = userDetail.DateOfBirth,

                PhoneNumber = userDetail.PhoneNumber,
            };

            return Ok(userDetailDto);
        }
        [Authorize(Roles = "User")]
        [HttpPost("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("ფაილი არ აირჩიე");

            var uploadsFolder = Path.Combine("wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var userDetail = _data.UserDetails.FirstOrDefault(x => x.UserId == currentUserId);
            if (userDetail == null)
                return BadRequest("User Detail not found");


            userDetail.UserProfileImg = $"/uploads/{fileName}";
            _data.SaveChanges();


            var imageUrl = $"{Request.Scheme}://{Request.Host}{userDetail.UserProfileImg}";

            return Ok(new { imageUrl });
        }


        #endregion
    }
}
