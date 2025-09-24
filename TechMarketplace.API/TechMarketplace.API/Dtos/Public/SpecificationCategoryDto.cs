namespace TechMarketplace.API.Dtos.Public
{
    public class SpecificationCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SpecificationDto> Specifications { get; set; } = new List<SpecificationDto>();
    }
}
