using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechMarketplace.API.Data;
using TechMarketplace.API.Dtos.Fillters;

namespace TechMarketplace.API.Controllers.FilterControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillterController : ControllerBase
    {

        private readonly DataContext _data;


        public FillterController(DataContext data)
        {
              _data = data;
        }


        [HttpGet("Product-GteBy/{name}")]
        public ActionResult SearchByName(string name)
        {
            var search = _data.Products.Where(x => x.Name.Contains(name)).
                Select(x => new SerchByName
                {
                    Name = x.Name

                }).ToList();

            return Ok(search);  

           

         
        }


    }
}
