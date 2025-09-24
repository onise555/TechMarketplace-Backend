using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.WishLists
{
    public class WishListItem
    {
        public int Id { get; set; }

        public int WishListId { get; set; }
        public WishList WishList { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }

   
        public int Quantity { get; set; }  =1 ;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
  



    }
}
