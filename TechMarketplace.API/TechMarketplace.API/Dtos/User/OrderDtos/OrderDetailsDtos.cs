namespace TechMarketplace.API.Dtos.User.OrderDtos
{
    public class OrderDetailsDtos
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderAddressDtos Address { get; set; }
        public List<OrderItemDtos> Items { get; set; } =new List<OrderItemDtos>();
    }
}
