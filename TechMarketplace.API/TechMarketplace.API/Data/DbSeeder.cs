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
                    FirstName = "SuperAdmin",
                    LastName ="AD",
                    Email = "admin@system.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"), // ძლიერი პაროლი
                    Role = UserRole.Admin,
                    IsVerified = true,
                    VerifyCode = "0000",
                    IsActive = true,
                     
                }
            );
        }
    }
}
