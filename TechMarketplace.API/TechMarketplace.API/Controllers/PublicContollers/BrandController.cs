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
    public class BrandController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;


        public BrandController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;

        }


        [HttpGet("Get-All-Brands")]
        public ActionResult GetAllBrands()
        {
            var brand = _data.Brands.Select(x => new BrandDtos
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Description = x.Description,
                CreatedAt = DateTime.Now,
            }).ToList();

            return Ok(brand);
        }


        [HttpGet("Get-Brand/{id}/Products")]
        public ActionResult GetBrandProducts(int id)
        {
            var BrandProducts = _data.Brands.Include(x => x.products)
                .Where(x => x.Id == id)
                .Select(x => new GetBrandAllProducts
                {
                    BrandDtos = new BrandDtos
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ImageUrl = x.ImageUrl,
                        Description = x.Description,
                        CreatedAt = DateTime.Now,

                    },
                    ProductDtos = x.products.Select(x => new ProductDtos
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ProductImgUrl = x.ProductImgUrl,
                        CreatedAt = DateTime.Now,
                        Price = x.Price,
                        Model = x.Model,
                        Sku = x.Sku,
                        Status = x.Status,

                    }).ToList()
                }).FirstOrDefault();

            return Ok(BrandProducts);
        }
    }

}