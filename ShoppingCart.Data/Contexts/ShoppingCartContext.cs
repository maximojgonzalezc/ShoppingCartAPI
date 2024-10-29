using Microsoft.EntityFrameworkCore;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Data.Contexts
{
    public class ShoppingCartContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }

        // Método SeedData que carga los datos iniciales
        public void SeedData()
        {
            if (Products.Any())
                return; // Si ya existen productos, no hacer nada

            // Crear productos
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Cookie",
                    Price = 1.25,
                    SupportsBulkPricing = true,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday } // Descuento especial los viernes
                },
                new Product
                {
                    Id = 2,
                    Name = "Key Lime Cheesecake",
                    Price = 8.00,
                    SpecificDate = new DateTime(DateTime.Now.Year, 10, 1) // Descuento especial el 1 de octubre
                },
                new Product
                {
                    Id = 3,
                    Name = "Mini Gingerbread Donut",
                    Price = 0.50,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday } // Descuento especial los martes
                },
                new Product
                {
                    Id = 4,
                    Name = "Brownie",
                    Price = 2.00,
                    SupportsBulkPricing = true
                }
            };

            Products.AddRange(products);
            SaveChanges();

            // Recuperar los productos ya guardados
            var cookie = Products.First(p => p.Name == "Cookie");
            var cheesecake = Products.First(p => p.Name == "Key Lime Cheesecake");
            var donut = Products.First(p => p.Name == "Mini Gingerbread Donut");
            var brownie = Products.First(p => p.Name == "Brownie");

            // Crear descuentos utilizando DiscountPercentage en lugar de DiscountPrice
            var discounts = new List<Discount>
            {
                new Discount
                {
                    ProductId = cookie.Id,
                    RequiredQuantity = 8,
                    DiscountPercentage = 0.40, // 40% de descuento para 8 cookies los viernes
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = cheesecake.Id,
                    RequiredQuantity = 1,
                    DiscountPercentage = 0.25, // 25% de descuento el 1 de octubre
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = donut.Id,
                    RequiredQuantity = 2,
                    DiscountPercentage = 0.50, // Dos por uno los martes (50% de descuento)
                    DiscountType = (int)DiscountType.SpecialDay
                },
                new Discount
                {
                    ProductId = cookie.Id,
                    RequiredQuantity = 6,
                    DiscountPercentage = 0.20, // Descuento por cantidad (6 por 20% menos)
                    DiscountType = (int)DiscountType.Bulk
                },
                new Discount
                {
                    ProductId = brownie.Id,
                    RequiredQuantity = 4,
                    DiscountPercentage = 0.125, // Descuento por cantidad (4 brownies por $7)
                    DiscountType = (int)DiscountType.Bulk
                }
            };

            Discounts.AddRange(discounts);
            SaveChanges();
        }
    }
}