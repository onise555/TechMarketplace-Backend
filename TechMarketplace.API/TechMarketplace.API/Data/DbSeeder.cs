using Microsoft.EntityFrameworkCore;
using System.Data;
using TechMarketplace.API.Models.Users;

namespace TechMarketplace.API.Data
{
    public class DbSeeder
    {

        public static void Seed(ModelBuilder modelBuilder)
        {
            // Default Admin
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Anonymus",
                    LastName = "secret",
                    Email = "tsotskhalashvili555@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Anonymus@123"),
                    Role = UserRole.SuperAdmin,
                    IsVerified = true,
                    VerifyCode = "0000",
                    IsActive = true,
                }
            );

            // Admin Detail
            modelBuilder.Entity<UserDetail>().HasData(
                new UserDetail
                {
                    Id = 1,
                    UserId = 1,
                    PhoneNumber = "+995000000000",
                    UserProfileImg = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlzjx4EzLW5lpMnk8NHtGbVV57e0whP0aHr5mE7zUUJjiLSVeofAWZQCM&s",
                    DateOfBirth = new DateTime(2000, 1, 1)
                }
            );
        }

    }
}
