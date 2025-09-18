namespace TechMarketplace.API.Requests.Admin.AdminSubCategoryRequest
{
    public class UpdateSubCategoryRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}
