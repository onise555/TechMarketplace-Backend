namespace TechMarketplace.API.Models.Users
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Street { get; set; } 
        public string City { get; set; } 
        public string ZipCode { get; set; } 
        public bool IsDefault { get; set; }
        public string? Label { get; set; }

   
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
