namespace TechMarketplace.API.Requests.Admin.AdminCategoryRequests
{
    public class CreateCatrgoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }

    }
}
