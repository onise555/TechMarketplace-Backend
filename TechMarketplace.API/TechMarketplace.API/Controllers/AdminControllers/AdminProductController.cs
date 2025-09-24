using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminProductDtos;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminProductRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminProductController : ControllerBase
    {

        private readonly DataContext _data;
        private readonly JwtServices _Jwt;
        private readonly EmailSender _emailSender;

        public AdminProductController(DataContext data, JwtServices jwt, EmailSender emailSender)
        {
            _data = data;
            _Jwt = jwt;
            _emailSender = emailSender;
        }

       

        //AddProduct
        [HttpPost("Add-Product")]

        public async Task<ActionResult> AddProduct([FromForm] CreateProductRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("ფაილი არ აირჩიე");

            var uploadsFolder = Path.Combine("wwwroot/uploads/products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            Product product = new Product()
            {
                ProductImgUrl = $"/uploads/products/{fileName}",
                Name = request.Name,
                Status = request.Status,
                Price = request.Price,
                Model = request.Model,
                CreatedAt = DateTime.Now,
                Sku = request.Sku,
                BrandId = request.BrandId,
                SubCategoryId = request.SubCategoryId,
            };

            _data.Products.Add(product);
            _data.SaveChanges();


            var imageUrl = $"{Request.Scheme}://{Request.Host}{product.ProductImgUrl}";

            return Ok(product);
        }

         
        // Update-Product/Id

        [HttpPut("Update-Product/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromForm] UpdateProductRequest req)
        {
            var p = _data.Products.FirstOrDefault(x => x.Id == id);
            if (p == null)
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
                    await req.File.CopyToAsync(stream);
                }

                p.ProductImgUrl = $"/uploads/products/{fileName}";
            }

            p.Name = req.Name;
            p.Status = req.Status;
            p.Sku = req.Sku;
            p.Model = req.Model;
            p.Price = req.Price;
            p.BrandId = req.BrandId;
            p.SubCategoryId = req.SubCategoryId;


            _data.SaveChanges();

            var imageUrl = string.IsNullOrEmpty(p.ProductImgUrl)
                ? null
                : $"{Request.Scheme}://{Request.Host}{p.ProductImgUrl}";

            var UpdateProductDto = new UpdateProductDtos
            {
                Id = id,
                Name = p.Name,
                CreatedAt = p.CreatedAt,
                Model = p.Model,
                Price = p.Price, 
                ProductImgUrl = p.ProductImgUrl,
                Sku = p.Sku,
                Status = p.Status,
            };

            return Ok(UpdateProductDto);
        }


        [HttpPut("Update-Product/{id}/Status")]
        public ActionResult UpdateStatus(int id, UpdateStatusRequest req)
        {
            var UpdateStatus = _data.Products.Where(x => x.Id == id).FirstOrDefault();

            if (UpdateStatus == null)
            {
                return NotFound("Product Not Found");
            }

            UpdateStatus.Status = req.Status;

            _data.SaveChanges();

            var UpdateStatusDto = new UpdateStatusDtos
            {
                Id = id,
                Name = UpdateStatus.Name,
                Status = UpdateStatus.Status,
            };

            return Ok(UpdateStatusDto);



        }

        [HttpDelete("Delete-Product/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var p =_data.Products.FirstOrDefault(x => x.Id == id);
            
            if (p == null)
            {
                return NotFound("Product Not Founded");
            }

            _data.Products.Remove(p);
            _data.SaveChanges();

            var DeleteDto = new ProductDeleteDtos
            {
                Id = id,
            };

            return Ok(new
            {
                mewssage = $"Delete This {DeleteDto.Id} Product"
            });
        }
    }
}