namespace TechMarketplace.API.Requests.Admin.AdminProductRequests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public ProductStatus Status { get; set; }
        public IFormFile? File { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public string Model { get; set; }
        public string Sku { get; set; }

        public int SubCategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
