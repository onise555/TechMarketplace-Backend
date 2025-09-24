using TechMarketplace.API.Models.Orders;

namespace TechMarketplace.API.Models.Payments
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionId { get; set; }
        public PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}
