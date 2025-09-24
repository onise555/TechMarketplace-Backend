namespace TechMarketplace.API.Models.Products
{
    public class SpecificationCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int productDetailid { get; set; }
        public ProductDetail productDetail { get; set; }    
        public List<ProductSpecification> Specifications { get; set; }  

    }
}
