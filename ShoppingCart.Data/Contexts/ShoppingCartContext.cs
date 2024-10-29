using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Models;

namespace ShoppingCart.Data.Contexts;

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

        // Definir los productos
        var products = new List<Product>
    {
        new Product
        {
            Name = "Brownie",
            Price = 2.00,
            ImageURL = "https://static.itdg.com.br/images/640-400/0191a4f23349e54e618a65f2051d68a8/shutterstock-1915577575-2-.jpg"
        },
        new Product
        {
            Name = "Key Lime Cheesecake",
            Price = 8.00,
            ImageURL = "http://1.bp.blogspot.com/-7we9Z0C_fpI/T90JXcg3YsI/AAAAAAAABn4/EN7u2vMuRug/s1600/key+lime+cheesecake+slice+in+front.jpg",
            SpecificDate = new DateTime(DateTime.Now.Year, 10, 1) // Ejemplo de fecha específica
        },
        new Product
        {
            Name = "Cookie",
            Price = 1.25,
            ImageURL = "http://www.mayheminthekitchen.com/wp-content/uploads/2015/05/chocolate-cookie-square.jpg",
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday }
        },
        new Product
        {
            Name = "Mini Gingerbread Donut",
            Price = 0.50,
            ImageURL = "https://pinchofyum.com/wp-content/uploads/gingerbread-donuts-4.jpg",
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday }
        }
    };

        // Guardar productos
        Products.AddRange(products);
        SaveChanges();

        // Recuperar productos para asignar descuentos
        var cookie = Products.First(p => p.Name == "Cookie");
        var cheesecake = Products.First(p => p.Name == "Key Lime Cheesecake");
        var donut = Products.First(p => p.Name == "Mini Gingerbread Donut");
        var brownie = Products.First(p => p.Name == "Brownie");

        // Crear descuentos específicos para cada producto
        var discounts = new List<Discount>
    {
        // Descuento para "Cookie" - Special Day y Bulk
        new Discount
        {
            ProductId = cookie.Id,
            RequiredQuantity = 8,
            DiscountPercentage = 0.40,
            DiscountType = (int)DiscountType.SpecialDay
        },
        new Discount
        {
            ProductId = cookie.Id,
            RequiredQuantity = 6,
            DiscountPercentage = 0.20,
            DiscountType = (int)DiscountType.Bulk
        },
        // Descuento para "Key Lime Cheesecake" - Special Day
        new Discount
        {
            ProductId = cheesecake.Id,
            RequiredQuantity = 1,
            DiscountPercentage = 0.25,
            DiscountType = (int)DiscountType.SpecialDay
        },
        // Descuento para "Mini Gingerbread Donut" - Special Day
        new Discount
        {
            ProductId = donut.Id,
            RequiredQuantity = 2,
            DiscountPercentage = 0.50,
            DiscountType = (int)DiscountType.SpecialDay
        },
        // Descuento para "Brownie" - Bulk
        new Discount
        {
            ProductId = brownie.Id,
            RequiredQuantity = 4,
            DiscountPercentage = 0.125,
            DiscountType = (int)DiscountType.Bulk
        }
    };

        // Guardar descuentos
        Discounts.AddRange(discounts);
        SaveChanges();
    }
}