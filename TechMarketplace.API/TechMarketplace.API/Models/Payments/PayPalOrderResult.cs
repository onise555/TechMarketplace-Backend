namespace TechMarketplace.API.Models.Payments
{
    public class PayPalOrderResult
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string ApprovalUrl { get; set; }
    }
}
