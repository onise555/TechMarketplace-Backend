namespace TechMarketplace.API.Dtos.Admin.AdminProductSpecificationDetos
{
    public class ProductSpecificationDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int ProductDetailId { get; set; }
        public int SpecificationCategoryId { get; set; }
        public string SpecificationCategoryName { get; set; } = string.Empty;
    }
}
