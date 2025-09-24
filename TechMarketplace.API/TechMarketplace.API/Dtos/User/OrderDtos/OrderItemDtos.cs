using TechMarketplace.API.Dtos.Product;

namespace TechMarketplace.API.Dtos.User.OrderDtos
{
    public class OrderItemDtos
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public  ProductDtos ProductDtos { get; set; }

    }
}
