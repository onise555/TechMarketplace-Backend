using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminSubCategoryDtos;
using TechMarketplace.API.Dtos.Public;
using TechMarketplace.API.Models.Category;
using TechMarketplace.API.Requests.Admin.AdminSubCategoryRequest;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminSubCategoryController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public AdminSubCategoryController(DataContext data, JwtServices jwt, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwt;
            _emailSender = emailSender;
        }



        [HttpPost("Create-SubCategroy")]
        public ActionResult CreateSubCategory(CreateSubCategory request)
        {
            SubCategory subc = new SubCategory()
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId ,
            };
            _data.SubCategories.Add(subc);
            _data.SaveChanges();

            return Ok(subc);
        }


        [HttpPut("Update-SubCatgory/{id}")]
        public ActionResult UpdateSubCategory(int id, SubCategory subc)
        {

           var subCategory=_data.SubCategories.FirstOrDefault(x => x.Id == id);

            if (subCategory == null)
            {
                return NotFound("SubCategory Not Founded");
            }

            subCategory.Id = id;    
            subCategory.Name = subc.Name;   
            subCategory.Description = subc.Description;
            subCategory.CategoryId = subc.CategoryId;

            var subCategoryDtos = new SubCategoryDtos()
            {
                Id = subCategory.Id,
                Name = subCategory.Name,
                Description = subCategory.Description,

            };

            return Ok(subCategoryDtos);
        }


        [HttpDelete("Delete-SubCategory/{id}")]

        public ActionResult DeleteSubCategory(int id)
        {
            var subc =_data.SubCategories.FirstOrDefault(x=>x.Id == id);    

            if (subc == null)
            {
                return NotFound("Subcategory Not Founded");
            }
            _data.SubCategories.Remove(subc);
            _data.SaveChanges();

            var subDeleteDtos = new DeleteSubCategoryDtos()
            {
                Id = subc.Id,
            };

            return Ok(subDeleteDtos); 


        }


    }
}
