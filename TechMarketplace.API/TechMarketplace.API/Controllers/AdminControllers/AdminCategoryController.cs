using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminCategoryDtos;
using TechMarketplace.API.Models.Brands;
using TechMarketplace.API.Models.Category;
using TechMarketplace.API.Requests.Admin.AdminCategoryRequests;
using TechMarketplace.API.Requests.Admin.AdminSubCategoryRequest;
using TechMarketplace.API.Services;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly DataContext _data;
        private readonly JwtServices _Jwt;


        public AdminCategoryController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;

        }


        [HttpPost("Add-Category")]
        public ActionResult AddCategory(CreateCatrgoryRequest request)
        {

            if (request.File == null || request.File.Length == 0)
                return BadRequest("ფაილი არ აირჩიე");

            var uploadsFolder = Path.Combine("wwwroot/uploads/categorys");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                request.File.CopyToAsync(stream);
            }

            Category category = new Category()
            {
                Name = request.Name,
                Description = request.Description,

                CateogryImgUrl = $"/uploads/categorys/{fileName}"
            };

            _data.Categories.Add(category);
            _data.SaveChanges();
            var imageUrl = $"{Request.Scheme}://{Request.Host}{category.CateogryImgUrl}";
            return Ok(category);
        }




        [HttpPut("Update-Category/{id}")]
        public ActionResult UpdateCategory(int id, UpdateCategoryRequest req)
        {

            var category = _data.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("პროდუქტი ვერ მოიძებნა");

            if (req.File != null && req.File.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot/uploads/products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{req.File.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    req.File.CopyToAsync(stream);
                }

                category.CateogryImgUrl = $"/uploads/products/{fileName}";
            }

            category.Name = req.Name;
            category.Description = req.Description;


            _data.SaveChanges();

            var imageUrl = string.IsNullOrEmpty(category.CateogryImgUrl)
            ? null
           : $"{Request.Scheme}://{Request.Host}{category.CateogryImgUrl}";


            var CategoryDto = new UpdateCategoryDtos
            {
                Id = id,
                Name = category.Name,
                Description = category.Description,
                CateogryImgUrl = category.CateogryImgUrl,

            };
            return Ok(CategoryDto);
        }



        [HttpDelete("Delete-Category/{id}")]
        
        public ActionResult DeleteCategory(int id)
        {
            var category =_data.Categories.FirstOrDefault(c => c.Id == id); 

            if (category == null)
            {
                return NotFound("Category Not Founded");
            }

            _data.Categories.Remove(category);  
            _data.SaveChanges();

            var categoryDeleteDtos = new DeleteCategoryDtos 
            {
                Id=category.Id,
            };

            return Ok(categoryDeleteDtos);  
        }



    }
}
