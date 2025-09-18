using TechMarketplace.API.Models.Brands;
using TechMarketplace.API.Models.Category;
using TechMarketplace.API.Models.Orders;
using TechMarketplace.API.Models.Reviews;
using TechMarketplace.API.Models.WishLists;

namespace TechMarketplace.API.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProductStatus Status { get; set; }
        public string ProductImgUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Model { get; set; }
        public string Sku { get; set; }


        public int BrandId { get; set; }

        public Brand Brand { get; set; }
        public ProductDetail ProductDetail { get; set; }    

        public int SubCategoryId { get; set; }  

        public SubCategory SubCategory { get; set; }

        public List<WishListItem> WishListItems { get; set; } = new List<WishListItem>();

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public List<Review> reviews { get; set; } = new List<Review>();




    }
}
