using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminProductDetailImgDtos;
using TechMarketplace.API.Models.Category;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminDetailImgRequests;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProductImageController : ControllerBase
    {


        private readonly DataContext _data;
        private readonly EmailSender _emailSender;

        public AdminProductImageController(DataContext data, EmailSender emailSender)
        {
            _data = data;
            _emailSender = emailSender;

        }

        [HttpPost("Product-Detail-Imgs")]
        public ActionResult AddDetailImgs(CreateDetailImgRequest req)
        {
            if (req.File == null || req.File.Length == 0)
                return BadRequest("ფაილი არ აირჩიე");

            var uploadsFolder = Path.Combine("wwwroot/uploads/products/DetailImigs");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{req.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                req.File.CopyToAsync(stream);
            }

            ProductImage productImage = new ProductImage()
            {
                ProductDetailId = req.ProductDetailId,
                ImgUrl = $"wwwroot/uploads/products/DetailImigs{fileName}",
            };

            _data.ProductImages.Add(productImage);
            _data.SaveChanges();

            var imageUrl = $"{Request.Scheme}://{Request.Host}{productImage.ImgUrl}";

            return Ok(productImage);



        }

        [HttpPut("product-Deetai-img/{id}")]
        public ActionResult UpdateDetailImgs(int id, UpdateDetailImgRequest req)
        {
            var DetailImg = _data.ProductImages.FirstOrDefault(c => c.Id == id);

            if (DetailImg == null)
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

                DetailImg.ImgUrl = $"wwwroot/uploads/products/DetailImigs{fileName}";
            }

            DetailImg.ProductDetailId = id;

            _data.SaveChanges();

            var imageUrl = string.IsNullOrEmpty(DetailImg.ImgUrl)
          ? null
         : $"{Request.Scheme}://{Request.Host}{DetailImg.ImgUrl}";

            var detailimgDto = new UpdateProductDetailImgDtos
            {
                Id = id,
                ProductDetailId = DetailImg.ProductDetailId,
                ImgUrl = DetailImg.ImgUrl,
            };

            return Ok(detailimgDto);

        }


        [HttpDelete("Delete-Product-DetailImg/{id}")]
        public ActionResult DeleteProductImg(int id)
        {
            var img = _data.ProductImages.FirstOrDefault(x => x.Id == id);

            if (img == null)
            {
                NotFound("Product Not Founded");
            }

            var DeleteDto = new DeleteProductDetaiImgDtos
            {
                Id = id,
            };

           _data.ProductImages.Remove(img); 
           _data.SaveChanges();
            
            return Ok(DeleteDto);
        }

    }
}