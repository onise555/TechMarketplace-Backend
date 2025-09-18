using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminBrandDtos;
using TechMarketplace.API.Dtos.Admin.AdminProductDtos;
using TechMarketplace.API.Models.Brands;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminBrandRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminBrandController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
  

        public AdminBrandController(DataContext data, JwtServices jwt)
        {
            _data = data;
            _Jwt = jwt;
        
        }

        [HttpPost("Add-Product-Brand")]
        public ActionResult AddBrand(CreateBrandRequest request)
        {

            if (request.File == null || request.File.Length == 0)
                return BadRequest("ფაილი არ აირჩიე");

            var uploadsFolder = Path.Combine("wwwroot/uploads/brands");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                request.File.CopyToAsync(stream);
            }

            Brand brand = new Brand()
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.Now,
                ImageUrl = $"/uploads/brands/{fileName}"
            };

         _data.Brands.Add(brand);
         _data.SaveChanges();    
         var imageUrl = $"{Request.Scheme}://{Request.Host}{brand.ImageUrl}";
         return Ok(brand);
        }

        [HttpPut("Update-Product-Brand/{id}")]
        public ActionResult UpdateBrand(int id, UpdateBrandRequest req)
        {
            var b = _data.Brands.FirstOrDefault(b => b.Id == id);
            if (b == null)
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

                b.ImageUrl = $"/uploads/products/{fileName}";
            }



            b.Name = req.Name;
            b.Description = req.Description;

            _data.SaveChanges();

            var imageUrl = string.IsNullOrEmpty(b.ImageUrl)
                ? null
                : $"{Request.Scheme}://{Request.Host}{b.ImageUrl}";

            var UpdateProductDto = new UpdateBrandDtos
            {
                 Id=b.Id,
                 Name=b.Name,
                 Description=b.Description,
                 ImageUrl=b.ImageUrl,    
            };  
            
            return Ok(UpdateProductDto);
        }

        [HttpDelete("Delete-Brand/{id}")]

        public ActionResult DeleteBrand(int id)
        {
            var b =_data.Brands.FirstOrDefault(b=>b.Id == id);

            if (b == null)
            {
                return NotFound("Brand Not Founded");
            }

            _data.Brands.Remove(b);
            _data.SaveChanges();

            var BrandDeleteDtos = new DeleteBrandDtos 
            { 
                Id = b.Id,
            };

            return Ok(BrandDeleteDtos);

        }

    }
}
