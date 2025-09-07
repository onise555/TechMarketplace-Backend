namespace TechMarketplace.API.Requests.Admin
{
    public class UpdateUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public UserRole Role { get; set; }

    }
}
