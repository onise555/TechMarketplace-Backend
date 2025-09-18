namespace TechMarketplace.API.Dtos.User
{
    public class UpdateUserDetailDtos
    {
        public int Id { get; set; }

        public string UserProfileImg { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; }

    }
}
