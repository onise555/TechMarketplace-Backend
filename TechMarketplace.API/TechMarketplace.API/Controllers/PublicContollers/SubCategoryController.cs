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
    public class SubCategoryController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;


        public SubCategoryController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;

        }



        [HttpGet("Get-All-Subcategorys")]
        public ActionResult GetAllSubCategorys()
        {
            var subcategory=_data.SubCategories.Select(x=> new SubCategoryDtos
            {
                Id = x.Id,
                Name = x.Name,  
                Description = x.Description,    
            }).ToList();

            return Ok(subcategory); 
        }


        [HttpGet("Get-SubCategory-Products{id}")]
        public ActionResult GetProduct(int id)
        {
            var subcategoryWithProducts = _data.SubCategories
                  .Include(x => x.Products)
                  .ThenInclude(x => x.ProductDetail)
                  .Where(x => x.Id == id)
                  .Select(x => new GetAllSubCategoryProductsDtos
                  {
                      SubCategoryId = id,
                      Name = x.Name,
                      Description = x.Description,

                      ProductsDto = x.Products
                      .Where(x=> x.Status==ProductStatus.Active )
                      .Select(x => new ProductDtos
                      {
                          Id = x.Id,
                          ProductImgUrl = x.ProductImgUrl,
                          Name = x.Name,
                          Sku = x.Sku,
                          Status = x.Status,
                          CreatedAt = x.CreatedAt,
                          Model = x.Model,
                          Price = x.Price,

                      }).ToList()
                  }).FirstOrDefault();
            return Ok(subcategoryWithProducts);
        }
    }
}
