namespace TechMarketplace.API.Dtos.Admin
{
    public class UserDtos
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }

        public bool IsActive { get; set; }
    }
}
