using TechMarketplace.API.Models.Users;

namespace TechMarketplace.API.Models.Carts
{
    public class Cart
    {

        public int Id { get; set; } 

        public int UserId { get; set; } 

        public User User { get; set; }

        public DateTime CreateAt { get; set; }

        public List<CartItem> Items { get; set;} = new List<CartItem>();    
    }
}
