namespace TechMarketplace.API.Requests.User.AddressRequests
{
    public class CreateUserAddressRequest
    {
        public string Country { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public bool IsDefault { get; set; }
        public string? Label { get; set; }
        public int UserId { get; set; }
    }
}
