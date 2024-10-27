using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        // Definición de DbSet para las entidades
        public DbSet<Product> Products { get; set; }
        public DbSet<BulkPricing> BulkPricings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Opcional: Configuración de valores iniciales (Seed Data)
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Cookie", Price = 1.25 },
                new Product { Id = 2, Name = "Key Lime Cheesecake", Price = 8.00 },
                new Product { Id = 3, Name = "Mini Gingerbread Donut", Price = 0.50 },
                new Product { Id = 4, Name = "Brownie", Price = 2.00 }
            );
        }
    }
}
