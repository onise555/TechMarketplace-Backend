namespace TechMarketplace.API.Models.Products
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public List<ProductImage> Images { get; set; }  = new List<ProductImage>();
        public List<ProductSpecification> Specifications { get; set; } = new List<ProductSpecification>();






    }
}
