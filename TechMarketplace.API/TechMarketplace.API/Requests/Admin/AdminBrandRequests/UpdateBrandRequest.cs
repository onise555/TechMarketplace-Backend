namespace TechMarketplace.API.Requests.Admin.AdminBrandRequests
{
    public class UpdateBrandRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile File { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
