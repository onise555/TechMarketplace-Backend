namespace TechMarketplace.API.Dtos.User.AddressDtos
{
    public class UpdateUserAddressDto
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Label { get; set; }
        public bool IsDefault { get; set; }
    }
}
