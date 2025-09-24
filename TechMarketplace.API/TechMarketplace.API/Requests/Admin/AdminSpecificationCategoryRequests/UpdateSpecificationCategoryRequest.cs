namespace TechMarketplace.API.Requests.Admin.AdminSpecificationCategoryRequests
{
    public class UpdateSpecificationCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
