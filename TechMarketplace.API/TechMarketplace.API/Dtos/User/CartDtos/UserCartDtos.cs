using TechMarketplace.API.Dtos.User.CartItemDtos;

namespace TechMarketplace.API.Dtos.User.CartDtos
{
    public class UserCartDtos
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
