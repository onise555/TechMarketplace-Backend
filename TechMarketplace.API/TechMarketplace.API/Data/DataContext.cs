using Microsoft.EntityFrameworkCore;
using TechMarketplace.API.Models.Brands;
using TechMarketplace.API.Models.Carts;
using TechMarketplace.API.Models.Category;
using TechMarketplace.API.Models.Orders;
using TechMarketplace.API.Models.Payments;
using TechMarketplace.API.Models.Products;
using TechMarketplace.API.Models.Reviews;
using TechMarketplace.API.Models.Users;
using TechMarketplace.API.Models.WishLists;

namespace TechMarketplace.API.Data
{
    public class DataContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSpecification> ProductSpecification { get; set; }   
        public DbSet<SpecificationCategory> SpecificationCategories { get; set; }   
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Payment> Payments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TechMarketProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany() 
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            DbSeeder.Seed(modelBuilder);


        }



    }


}
