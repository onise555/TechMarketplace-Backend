using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminSpecificatioCategoryDtos;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminSpecificationCategoryRequests;

using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminSpecificationCategoryController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly EmailSender _emailSender;

        public AdminSpecificationCategoryController(DataContext data, EmailSender emailSender)
        {
            _data = data;
            _emailSender = emailSender;
        }
        [HttpPost("specification-category")]
        public ActionResult AddCategory(CreateSpecificationCategoryRequest req)
        {
            var category = new SpecificationCategory
            {
                Name = req.Name,
                productDetailid = req.productDetailId
            };

            _data.SpecificationCategories.Add(category);
            _data.SaveChanges();

            return Ok(category);
        }


        [HttpPut("specification-category/{id}")]
        public ActionResult UpdateCategory(int id, UpdateSpecificationCategoryRequest req)
        {
            var category = _data.SpecificationCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound("Category not found");

            category.Name = req.Name;
            _data.SaveChanges();

            return Ok(category);
        }



        [HttpDelete("specification-category/{id}")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _data.SpecificationCategories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound("Category not found");

            _data.SpecificationCategories.Remove(category);
            _data.SaveChanges();

            return Ok(category);
        }

    }
}
