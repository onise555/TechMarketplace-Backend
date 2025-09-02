using TechMarketplace.API.Models.Payments;
using TechMarketplace.API.Models.Users;

namespace TechMarketplace.API.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressId { get; set; }

        public Address Address { get; set; }        
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }



        public List<OrderItem> Items { get; set;} =new List<OrderItem>();

        public List<Payment> Payments { get; set; } = new List<Payment>();



    }
}
