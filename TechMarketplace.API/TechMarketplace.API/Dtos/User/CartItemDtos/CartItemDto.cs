namespace TechMarketplace.API.Dtos.User.CartItemDtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }  

        public CartItemProductDtos cartItemProductDtos { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
