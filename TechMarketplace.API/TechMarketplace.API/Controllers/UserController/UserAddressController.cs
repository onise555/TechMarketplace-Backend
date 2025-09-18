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
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Requests.User;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;
using TechMarketplace.API.Validators.User;
namespace TechMarketplace.API.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public UserAddressController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }



        #region User Addresses
        [Authorize(Roles = "User")]
        [HttpPost("User-addresses")]
        public ActionResult AddUserAddress(CreateUserAddressRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (req.UserId != currentUserId)
                return Forbid();

            if (req.IsDefault)
            {
                var existingDefault = _data.Addresses
                    .FirstOrDefault(a => a.UserId == currentUserId && a.IsDefault);

                if (existingDefault != null)
                    existingDefault.IsDefault = false;
            }





            var Address = new Address()
            {
                Country = req.Country,
                City = req.City,
                Street = req.Street,
                ZipCode = req.ZipCode,
                Label = req.Label,
                IsDefault = req.IsDefault,
                UserId = req.UserId,

            };
            _data.Addresses.Add(Address);
            _data.SaveChanges();
            return Ok(Address);




        }


        [HttpGet("Get-User-Addresses/{id}")]
        public ActionResult GetAddresses(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var addresses = _data.Addresses.Where(x => x.UserId == id)
                .Select(x => new UserAddressDtos
                {
                    Id = x.Id,
                    Country = x.Country,
                    City = x.City,
                    Street = x.Street,
                    ZipCode = x.ZipCode,
                    Label = x.Label,
                    IsDefault = x.IsDefault,

                }).ToList();

            return Ok(addresses);
        }


        [Authorize(Roles = "User")]
        [HttpPut("Update-User-Address/{id}")]
        public ActionResult UpdateUserAddress(int id, CreateUserAddressRequest req)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var address = _data.Addresses.FirstOrDefault(a => a.Id == id);

            if (address == null)
                return NotFound("Address not found");

            if (address.UserId != currentUserId)
                return Forbid();


            if (req.IsDefault)
            {
                var existingDefault = _data.Addresses.FirstOrDefault(a => a.UserId == currentUserId && a.IsDefault && a.Id != id);
                if (existingDefault != null)
                    existingDefault.IsDefault = false;
            }


            address.Country = req.Country;
            address.City = req.City;
            address.Street = req.Street;
            address.ZipCode = req.ZipCode;
            address.Label = req.Label;
            address.IsDefault = req.IsDefault;

            _data.SaveChanges();


            var addressDto = new UpdateUserAddressDto
            {
                Id = address.Id,
                Country = address.Country,
                City = address.City,
                Street = address.Street,
                ZipCode = address.ZipCode,
                Label = address.Label,
                IsDefault = address.IsDefault
            };

            return Ok(addressDto);
        }




        [Authorize(Roles = "User")]
        [HttpDelete("User-Address/{id}")]
        public ActionResult DeleteAddress(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);



            var Address = _data.Addresses.FirstOrDefault(x => x.Id == id);

            if (Address == null)
                return BadRequest("Address Not Founded");

            if (Address.UserId != currentUserId)
                return Forbid();

            _data.Addresses.Remove(Address);
            _data.SaveChanges();

            if (Address.IsDefault)
            {
                var anotherAddress = _data.Addresses
                    .Where(a => a.UserId == currentUserId)
                    .OrderBy(a => a.Id)
                    .FirstOrDefault();

                if (anotherAddress != null)
                {
                    anotherAddress.IsDefault = true;
                    _data.SaveChanges();
                }
            }


            var AddressDeleteDtos = new DeleteAddressDtos()
            {
                Id = Address.Id,
            };

            return Ok(AddressDeleteDtos);
        }

        #endregion


    }
}
