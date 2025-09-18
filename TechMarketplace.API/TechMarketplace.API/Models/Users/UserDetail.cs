namespace TechMarketplace.API.Models.Users
{
    public class UserDetail
    {
        public int Id { get; set; } 

        public string UserProfileImg { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; } 

        public int UserId {  get; set; }    

        public User User { get; set; }
    }
}
