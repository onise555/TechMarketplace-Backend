using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Admin.AdminProductSpecificationDetos;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Requests.Admin.AdminProductSpecificationRequests;
using TechMarketplace.API.Requests.Admin.AdminProductSpecificationRequests;
using TechMarketplace.API.SMTP;

namespace TechMarketplace.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProductSpecificationController : ControllerBase
    {


        private readonly DataContext _data;
        private readonly EmailSender _emailSender;

        public AdminProductSpecificationController(DataContext data, EmailSender emailSender)
        {
            _data = data;
            _emailSender = emailSender;

        }

        [HttpPost("product-specification")]
        public ActionResult AddSpecification(SpecificationRequest req)
        {
            var spec = new ProductSpecification
            {
                Key = req.Key,
                Value = req.Value,
                SpecificationCategoryId = req.SpecificationCategoryId
            };

            _data.ProductSpecification.Add(spec);
            _data.SaveChanges();

            return Ok(spec);
        }

        [HttpPut("product-specification/{id}")]
        public ActionResult UpdateSpecification(int id, UpdateSpecificationRequest req)
        {
            var spec = _data.ProductSpecification.FirstOrDefault(x => x.Id == id);
            if (spec == null) return NotFound("Specification not found");

            spec.Key = req.Key;
            spec.Value = req.Value;
            spec.SpecificationCategoryId = req.SpecificationCategoryId;

            _data.SaveChanges();

            return Ok(spec);
        }

        [HttpDelete("product-specification/{id}")]
        public ActionResult DeleteSpecification(int id)
        {
            var spec = _data.ProductSpecification.FirstOrDefault(x => x.Id == id);
            if (spec == null) return NotFound("Specification not found");

            _data.ProductSpecification.Remove(spec);
            _data.SaveChanges();

            return Ok(spec);
        }
    }

}

