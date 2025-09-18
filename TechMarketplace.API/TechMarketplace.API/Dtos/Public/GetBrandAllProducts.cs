using TechMarketplace.API.Dtos.Product;

namespace TechMarketplace.API.Dtos.Public
{
    public class GetBrandAllProducts
    {
         public BrandDtos BrandDtos { get; set; }

        public List<ProductDtos> ProductDtos { get; set; }


    }
}
