using System.ComponentModel.DataAnnotations;

namespace TechMarketplace.API.Models.Products
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;


        public int SpecificationCategoryId { get; set; }
        public SpecificationCategory SpecificationCategory { get; set; } = null!;
    }
}
