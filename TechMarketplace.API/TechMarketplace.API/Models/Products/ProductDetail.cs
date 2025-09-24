    namespace TechMarketplace.API.Models.Products
    {
        public class ProductDetail
        {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
        public List<SpecificationCategory> SpecificationCategories { get; set; } = new List<SpecificationCategory>();


    }
}
