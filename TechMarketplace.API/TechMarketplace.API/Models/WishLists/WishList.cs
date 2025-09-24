using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Models.Users;

namespace TechMarketplace.API.Models.WishLists
{
    public class WishList
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public List<WishListItem> wishListItems { get; set; } = new List<WishListItem>();   



    }
}
