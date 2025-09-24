namespace TechMarketplace.API.Requests.Admin.AdminProductSpecificationRequests
{
    public class SpecificationRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int SpecificationCategoryId { get; set; }
    }
}
