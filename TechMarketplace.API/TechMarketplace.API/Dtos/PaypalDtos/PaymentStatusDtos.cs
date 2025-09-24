namespace TechMarketplace.API.Dtos.PaypalDtos
{
    public class PaymentStatusDtos
    {
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid => Status == PaymentStatus.Completed;
    }
}
