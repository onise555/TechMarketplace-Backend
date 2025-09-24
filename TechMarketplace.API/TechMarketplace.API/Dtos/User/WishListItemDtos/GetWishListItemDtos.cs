using TechMarketplace.API.Dtos.Product;

namespace TechMarketplace.API.Dtos.User.WishListItemDtos
{
    public class GetWishListItemDtos
    {
        public int Id { get; set; }
        public int WishListId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductDtos Product { get; set; }    

    }
}
