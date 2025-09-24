namespace TechMarketplace.API.Dtos.PaypalDtos
{
    public class PaymentDtos
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaymentUrl { get; set; } // PayPal URL
    }
}
