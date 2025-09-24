namespace TechMarketplace.API.Dtos.User.AddressDtos
{
    public class UserAddressDtos
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public bool IsDefault { get; set; }
        public string? Label { get; set; }
    }
}
