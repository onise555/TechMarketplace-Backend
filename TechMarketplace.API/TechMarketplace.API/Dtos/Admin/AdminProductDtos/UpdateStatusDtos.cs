namespace TechMarketplace.API.Dtos.Admin.AdminProductDtos
{
    public class UpdateStatusDtos
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public ProductStatus Status { get; set; }   
    }
}
