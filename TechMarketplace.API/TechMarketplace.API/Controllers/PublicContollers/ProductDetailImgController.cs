using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Public;
using TechMarketplace.API.Services;

namespace TechMarketplace.API.Controllers.PublicContollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailImgController : ControllerBase
    {

        private readonly DataContext _data;

        public ProductDetailImgController(DataContext data)
        {
            _data = data;
        }


        [HttpGet("Product-DetailImgs/{id}")]
        public ActionResult GetProductDetaiImgs(int id)
        {
            var detailImgs = _data.ProductImages.Where(x => x.ProductDetailId==id).Select(x => new ProductDetailimgsDtos
            {
                Id = x.Id,
                ImgUrl = x.ImgUrl,
                ProductDetailId = x.ProductDetailId,    
            }).ToList();

            if (detailImgs == null)
            {
                NotFound("detai Imgs  Not founded");
            }

            return Ok(detailImgs);

        }
    }
}