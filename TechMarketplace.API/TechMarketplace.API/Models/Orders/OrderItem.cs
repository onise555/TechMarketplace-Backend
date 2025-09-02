using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public decimal Price { get; set; }  

        public int Quantity { get; set; }

        public decimal TotalPrice => Price * Quantity;

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        
    }
}
