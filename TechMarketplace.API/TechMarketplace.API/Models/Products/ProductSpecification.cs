namespace TechMarketplace.API.Models.Products
{
    public class ProductSpecification
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }
    }
}
