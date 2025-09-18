using TechMarketplace.API.Dtos.Admin.AdminProductDtos;
using TechMarketplace.API.Dtos.Product;

namespace TechMarketplace.API.Dtos.Public
{
    public class GetAllSubCategoryProductsDtos
    {
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

         public List<ProductDtos> ProductsDto { get; set; }
    }
}
