namespace TechMarketplace.API.Dtos.Admin.AdminProductDtos
{
    public class UpdateProductDtos
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public ProductStatus Status { get; set; }
        public string ProductImgUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Model { get; set; }
        public string Sku { get; set; }
    }
}
