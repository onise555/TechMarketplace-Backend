using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.Brands
{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> products { get; set; } = new List<Product>();  

        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } 



    }
}
