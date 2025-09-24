namespace TechMarketplace.API.Requests.Admin.AdminProductDetailRequests
{
    public class CreateProductDetailRequest
    {
        public int Stock { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }
    }
}
