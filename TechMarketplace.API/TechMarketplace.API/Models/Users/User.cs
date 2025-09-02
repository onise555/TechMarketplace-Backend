

using TechMarketplace.API.Models.Carts;
using TechMarketplace.API.Models.Orders;
using TechMarketplace.API.Models.Reviews;
using TechMarketplace.API.Models.WishLists;

namespace TechMarketplace.API.Models.Users
{
    public class User
    {
        public int Id { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserRole Role { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? VerifyCode { get; set; } 
        public bool IsVerified { get; set; } = false;

        public DateTime? VerifyCodeExpiresAt { get; set; }

        public UserDetail UserDetail { get; set; }
        
        public Cart Cart { get; set; }

        public List<Address> AddressList { get; set; }  = new List<Address>();

        public List<Order> orders { get; set; } = new List<Order>();

        public List<WishList> WishList { get; set; } =new List<WishList>();

        public List<Review> reviews { get; set; }   = new List<Review>(); 


         




    }
}
