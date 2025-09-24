using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.Category
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
