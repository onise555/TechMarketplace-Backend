namespace TechMarketplace.API.Requests.Admin.AdminSpecificationCategoryRequests
{
    public class CreateSpecificationCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public int productDetailId { get; set; }
    }
}
