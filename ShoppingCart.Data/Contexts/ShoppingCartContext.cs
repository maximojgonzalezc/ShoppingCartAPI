using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<BulkPricing> BulkPricings { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Models.ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración de relaciones y datos iniciales específicos se omiten por brevedad
        }

        public void SeedData()
        {
            //// Verificar si ya existen datos en la base de datos
            //if (Products.Any() && Discounts.Any() && BulkPricings.Any()) return;

            //// Paso 1: Agregar los productos primero
            //var products = new List<Product>
            //{
            //    new Product { Id = 1, Name = "Cookie", Price = 1.25 },
            //    new Product { Id = 2, Name = "Key Lime Cheesecake", Price = 8.00 },
            //    new Product { Id = 3, Name = "Mini Gingerbread Donut", Price = 0.50 },
            //    new Product { Id = 4, Name = "Brownie", Price = 2.00 }
            //};

            //Products.AddRange(products);
            //SaveChanges(); // Guardar para obtener los IDs generados por la base de datos

            //// Recuperar productos con sus IDs ya generados
            //var cookie = Products.First(p => p.Name == "Cookie");
            //var cheesecake = Products.First(p => p.Name == "Key Lime Cheesecake");
            //var donut = Products.First(p => p.Name == "Mini Gingerbread Donut");

            //// Paso 2: Agregar BulkPricing y Discounts usando productos persistidos
            //var bulkPricings = new List<BulkPricing>
            //{
            //    new BulkPricing { Id = 1, ProductId = cookie.Id, Product = cookie, Amount = 6, TotalPrice = 6.00 }
            //};

            //var discounts = new List<DiscountDto>
            //{
            //    new Discount
            //    {
            //        Id = 1,
            //        ProductId = cookie.Id,
            //        Product = cookie,
            //        DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday },
            //        RequiredQuantity = 8,
            //        DiscountPrice = 6.00 // Precio especial para 8 cookies los viernes
            //    },
            //    new DiscountDto
            //    {
            //        Id = 2,
            //        ProductId = cheesecake.Id,
            //        Product = cheesecake,
            //        SpecificDate = new DateTime(DateTime.Now.Year, 10, 1),
            //        DiscountPercentage = 0.25 // 25% de descuento en octubre
            //    },
            //    new Discount
            //    {
            //        Id = 3,
            //        ProductId = donut.Id,
            //        Product = donut,
            //        DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday },
            //        RequiredQuantity = 2,
            //        DiscountPrice = 0.50 // Dos por uno
            //    }
            //};

            //BulkPricings.AddRange(bulkPricings);
            //Discounts.AddRange(discounts);

            //// Guardar todos los datos
            //SaveChanges();
        }
    }
}