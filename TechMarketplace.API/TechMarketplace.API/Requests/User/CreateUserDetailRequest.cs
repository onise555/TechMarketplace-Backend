namespace TechMarketplace.API.Requests.User
{
    public class CreateUserDetailRequest
    {
        public string UserProfileImg { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }


        public int UserId { get; set; }
    }
}
