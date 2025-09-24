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
    public class ProductDetailController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public ProductDetailController(DataContext data, JwtServices jwtServices, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwtServices;
            _emailSender = emailSender;

        }

        [HttpGet("Get-Product-Detail/{id}")]

        public ActionResult GetDetail(int id)
        {
            var detil =_data.ProductDetails
                .Where(x=>x.Id == id)
                .Select(x=> new ProductDetaildtos
                {
                    Id = x.Id,  
                    Stock = x.Stock,
                    Description = x.Description,
                    ProductId = x.ProductId,
                }).FirstOrDefault();    


            if (detil == null)
            {
                return NotFound("Product Detail Not Founded");
            }

              

            return Ok(detil);

           

        }
    }
}
