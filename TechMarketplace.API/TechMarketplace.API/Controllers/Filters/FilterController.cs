using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Filters;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.Filters
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public FilterController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }


        [HttpGet("search-by/{name}")]
        public ActionResult SearchByname(string name)
        {
            var search= _data.Products.Where(x=> x.Name.Contains(name))
                .ToList().Select(x=> new SearcByName
                {
                    Id= x.Id,   
                    Name= x.Name,
                    Price= x.Price,
                    ImageUrl =x.ProductImgUrl,
                }).ToList();

            return Ok(search);
        }


        [HttpGet("SearchBy-Price/{from}/{to}")]
        public ActionResult SearchByprice(decimal from, decimal to)
        {

            var searchYear = _data.Products.Where(x => x.Price >= from && x.Price <= to)
                 .ToList().Select(x => new SearchByPrice
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Price = x.Price,
                     img = x.ProductImgUrl,
                 }).ToList();

            return Ok(searchYear);  

        }
    }
}
