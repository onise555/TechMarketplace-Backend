namespace TechMarketplace.API.Requests.User.PaymentRequests
{
    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = "PayPal";
    }
}
