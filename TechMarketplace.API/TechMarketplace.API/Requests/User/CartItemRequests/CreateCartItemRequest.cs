namespace TechMarketplace.API.Requests.User.CartItemRequests
{
    public class CreateCartItemRequest
    {
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
