using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Product;
using TechMarketplace.API.Dtos.Public;
using TechMarketplace.API.Services;

namespace TechMarketplace.API.Controllers.PublicContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;


        public ProductController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;

        }


        [HttpGet("Get-All-Producs")]
        public ActionResult GetProducts()
        {
            var product = _data.Products.
                Include(x=>x.ProductDetail)
                .Where(x => x.Status == ProductStatus.Active)
                .Select(x=> new ProductDtos
            {
                Id=x.Id,
                Name=x.Name,
                ProductImgUrl=x.ProductImgUrl,
                CreatedAt=x.CreatedAt,
                Model= x.Model,
                Price= x.Price,  
                Sku= x.Sku,
                Status = x.ProductDetail.Stock > 0
                        ? ProductStatus.Active
                        : ProductStatus.OutOfStock

                }).ToList();

            if (product == null)
            {
               NotFound("Product Not Founded");
            }
            return Ok(product);
        }


    }
}
    