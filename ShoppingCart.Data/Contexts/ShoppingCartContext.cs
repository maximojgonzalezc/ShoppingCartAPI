using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<BulkPricing> BulkPricings { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product-BulkPricing relationship
            modelBuilder.Entity<Product>()
                .OwnsOne(p => p.BulkPricing); // BulkPricing is a value object, owned by Product

            // Configure Product entity
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Cookie", Price = 1.25 },
                new Product { Id = 2, Name = "Key Lime Cheesecake", Price = 8.00 },
                new Product { Id = 3, Name = "Mini Gingerbread Donut", Price = 0.50 },
                new Product { Id = 4, Name = "Brownie", Price = 2.00 }
            );

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany() // No navigation property back to CartItem in Product
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ShoppingCartItem entity
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany() // No navigation property back to ShoppingCartItem in Product
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data example for Product with BulkPricing (optional)
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Cookie",
                    Price = 1.25,
                    BulkPricing = new BulkPricing { Amount = 10, TotalPrice = 10 }
                },
                new Product
                {
                    Id = 2,
                    Name = "Key Lime Cheesecake",
                    Price = 8.00,
                    BulkPricing = new BulkPricing { Amount = 5, TotalPrice = 35 }
                }
            );
        }
    }
}
