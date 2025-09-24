namespace TechMarketplace.API.Requests.User.OrderRequests
{
    public class CreateOrderRequest
    {
        public int AddressId { get; set; }
        public string? Notes { get; set; }
    }
}
