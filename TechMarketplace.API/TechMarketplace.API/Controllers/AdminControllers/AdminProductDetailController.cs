using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminProductDetailDtos;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Requests.Admin.AdminProductDetailRequests;
using TechMarketplace.API.Services;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminProductDetailController : ControllerBase
    {


        private readonly DataContext _data;
        private readonly EmailSender _emailSender;

        public AdminProductDetailController(DataContext data, EmailSender emailSender)
        {
            _data = data;
            _emailSender = emailSender;

        }


        [HttpPost("product-detail")]
        public ActionResult AddProductDetail(CreateProductDetailRequest req)
        {

         ProductDetail produactDetail  = new ProductDetail()
         {
            Stock = req.Stock,
            Description = req.Description,  
            ProductId = req.ProductId,
         };

            _data.ProductDetails.Add(produactDetail);
            _data.SaveChanges();

            return Ok(produactDetail);
        }


        [HttpPut("product-detail/{id}")]
        public ActionResult UpdateProductDtail(int id, UpdateProductDetailRequest req)
        {
            var productdetail = _data.ProductDetails.FirstOrDefault(x => x.ProductId == id);

            if (productdetail == null)
            {
                return NotFound(" Product detail not founded");
            }

            productdetail.Stock = req.Stock;
            productdetail.Description = req.Description;
            productdetail.ProductId = req.ProductId;

            _data.SaveChanges();

            var productdtailDto = new ProductDetailDtos
            {
                Id = id,
                ProductId = productdetail.ProductId,
                Description = productdetail.Description,
                Stock = productdetail.Stock,
            };

            return Ok(productdtailDto); 
        }

        [HttpDelete("product-detail/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var  detail = _data.ProductDetails.FirstOrDefault(x=>x.Id == id);

            if (detail == null)
            {
                return NotFound("product detail not founded");
            }
            _data.ProductDetails.Remove(detail);
            _data.SaveChanges();
            return Ok(detail);
        }
    }
}
