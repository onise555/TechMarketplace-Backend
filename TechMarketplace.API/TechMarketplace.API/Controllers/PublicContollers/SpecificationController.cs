using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Public;
using TechMarketplace.API.Services;

namespace TechMarketplace.API.Controllers.PublicContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;


        public SpecificationController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;

        }

        [HttpGet("product/{productDetailId}/categories")]
        public ActionResult GetCategoriesByProduct(int productDetailId)
        {
            var categories = _data.SpecificationCategories
                .Where(sc => sc.productDetailid == productDetailId)
                .Include(sc => sc.Specifications)
                .ToList();

            if (!categories.Any())
                return NotFound("No specification categories found for this product.");

            var result = categories.Select(sc => new SpecificationCategoryDto
            {
                Id = sc.Id,
                Name = sc.Name,
                Specifications = sc.Specifications.Select(s => new SpecificationDto
                {
                    Id = s.Id,
                    Key = s.Key,
                    Value = s.Value
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        [HttpGet("category/{categoryId}/specifications")]
        public ActionResult GetSpecificationsByCategory(int categoryId)
        {
            var category = _data.SpecificationCategories
                .Include(sc => sc.Specifications)
                .FirstOrDefault(sc => sc.Id == categoryId);

            if (category == null)
                return NotFound("Category not found.");

            var specsDto = category.Specifications.Select(s => new SpecificationDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value
            }).ToList();

            return Ok(specsDto);
        }

    }
}
