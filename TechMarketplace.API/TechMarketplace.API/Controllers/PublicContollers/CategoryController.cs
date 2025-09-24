using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Public;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.PublicContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public CategoryController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;
        }
        [HttpGet("Get-All-Categorys")]
        public ActionResult GetAllCategory()
        {
            var category=_data.Categories.Select(x=> new CategoryDtos
            {
                Id = x.Id,
                CateogryImgUrl = x.CateogryImgUrl,
                Name = x.Name,
                Description = x.Description,    
            }).ToList();

            return Ok(category);    
        }
        

    }
}
