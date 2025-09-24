namespace TechMarketplace.API.Dtos.Admin.AdminProductSpecificationDetos
{
    public class UpdateSpecificationDtos
    {
        public int Id { get; set; } 
        public string Key { get; set; }

        public string Value { get; set; }

        public int specificationCategoryId { get; set; }
    }
}
