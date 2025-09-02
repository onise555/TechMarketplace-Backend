namespace TechMarketplace.API.Models.Products
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImgUrl { get; set; }  

        public int ProductDetailId { get; set; }
        public ProductDetail ProductDetail { get; set; }

    }
}
