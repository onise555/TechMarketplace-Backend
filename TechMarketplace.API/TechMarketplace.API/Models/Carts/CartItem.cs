using TechMarketplace.API.Models.Products;

namespace TechMarketplace.API.Models.Carts
{
    public class CartItem
    {
        public int Id { get; set; } 

        public int CartId { get; set; } 

        public Cart Cart { get; set; }


        public int ProductId { get; set; }  

        public Product Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice => Product.Price * Quantity;
        public DateTime CreatedAt { get; set; } 

    }
}
