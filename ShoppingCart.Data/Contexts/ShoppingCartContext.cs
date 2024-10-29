using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ShoppingCart.Data.Contexts
{
    public class ShoppingCartContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Value converter for List<DayOfWeek> to store it as JSON
            var daysOfWeekConverter = new ValueConverter<List<DayOfWeek>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<DayOfWeek>>(v, (JsonSerializerOptions?)null) ?? new List<DayOfWeek>());

            // Value comparer for List<DayOfWeek> to compare collections correctly
            var daysOfWeekComparer = new ValueComparer<List<DayOfWeek>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            // Apply configurations to Product.DaysOfWeek only
            modelBuilder.Entity<Product>()
                .Property(p => p.DaysOfWeek)
                .HasConversion(daysOfWeekConverter)
                .Metadata.SetValueComparer(daysOfWeekComparer);

            // Set up foreign key relationship with cascade delete
            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Product)
                .WithMany(p => p.Discounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void SeedData()
        {
            if (Products.Any())
                return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Cookie",
                    Price = 1.25,
                    SupportsBulkPricing = true,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday }
                },
                new Product
                {
                    Name = "Key Lime Cheesecake",
                    Price = 8.00,
                    SpecificDate = new DateTime(DateTime.Now.Year, 10, 1)
                },
                new Product
                {
                    Name = "Mini Gingerbread Donut",
                    Price = 0.50,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday }
                },
                new Product
                {
                    Name = "Brownie",
                    Price = 2.00,
                    SupportsBulkPricing = true
                }
            };

            Products.AddRange(products);
            SaveChanges();

            // Recuperar los productos para asociar en descuentos
            var cookie = Products.First(p => p.Name == "Cookie");
            var cheesecake = Products.First(p => p.Name == "Key Lime Cheesecake");
            var donut = Products.First(p => p.Name == "Mini Gingerbread Donut");
            var brownie = Products.First(p => p.Name == "Brownie");

            // Crear descuentos sin referencia a DaysOfWeek en Discounts
            var discounts = new List<Discount>
            {
                new Discount
                {
                    ProductId = cookie.Id,
                    RequiredQuantity = 8,
                    DiscountPercentage = 0.40,
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = cheesecake.Id,
                    RequiredQuantity = 1,
                    DiscountPercentage = 0.25,
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = donut.Id,
                    RequiredQuantity = 2,
                    DiscountPercentage = 0.50,
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = cookie.Id,
                    RequiredQuantity = 6,
                    DiscountPercentage = 0.20,
                    DiscountType = (int)DiscountType.Bulk
                },
                new Discount
                {
                    ProductId = brownie.Id,
                    RequiredQuantity = 4,
                    DiscountPercentage = 0.125,
                    DiscountType = (int)DiscountType.Bulk
                }
            };

            Discounts.AddRange(discounts);
            SaveChanges();
        }
    }
}