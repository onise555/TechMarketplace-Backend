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
        public async Task<ActionResult> AddDetailImgs([FromForm] CreateDetailImgRequest req)
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
                await req.File.CopyToAsync(stream);
            }

            ProductImage productImage = new ProductImage()
            {
                ProductDetailId = req.ProductDetailId,
                ImgUrl = $"/uploads/products/DetailImigs/{fileName}"  // ✅ სლეში დამატებულია
            };

            _data.ProductImages.Add(productImage);
            _data.SaveChanges();

            var imageUrl = $"{Request.Scheme}://{Request.Host}{productImage.ImgUrl}";

            return Ok(new
            {
                productImage.Id,
                productImage.ProductDetailId,
                ImgUrl = imageUrl
            });
        }

        [HttpPut("product-Detail-img/{id}")]
        public async Task<ActionResult> UpdateDetailImgs([FromRoute] int id, [FromForm] UpdateDetailImgRequest req)
        {
            var detailImg = _data.ProductImages.FirstOrDefault(c => c.Id == id);

            if (detailImg == null)
                return NotFound("პროდუქტის სურათი ვერ მოიძებნა");

            if (req.File != null && req.File.Length > 0)
            {
                var uploadsFolder = Path.Combine("wwwroot/uploads/products/DetailImigs");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{req.File.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await req.File.CopyToAsync(stream);
                }

                detailImg.ImgUrl = $"/uploads/products/DetailImigs/{fileName}"; // ✅ სლეში დამატებულია
            }

            detailImg.ProductDetailId = req.ProductDetailId; // ✅ id-ს ნაცვლად req.ProductDetailId

            _data.SaveChanges();

            var imageUrl = string.IsNullOrEmpty(detailImg.ImgUrl)
                ? null
                : $"{Request.Scheme}://{Request.Host}{detailImg.ImgUrl}";

            var detailImgDto = new UpdateProductDetailImgDtos
            {
                Id = detailImg.Id,
                ProductDetailId = detailImg.ProductDetailId,
                ImgUrl = imageUrl
            };

            return Ok(detailImgDto);
        }

        [HttpDelete("Delete-Product-DetailImg/{id}")]
        public ActionResult DeleteProductImg(int id)
        {
            var img = _data.ProductImages.FirstOrDefault(x => x.Id == id);

            if (img == null)
                return NotFound("Product Not Found");

            var deleteDto = new DeleteProductDetaiImgDtos
            {
                Id = id,
            };

            _data.ProductImages.Remove(img);
            _data.SaveChanges();

            return Ok(deleteDto);
        }
    }
}