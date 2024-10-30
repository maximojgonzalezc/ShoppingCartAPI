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

    public ShoppingCartContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var daysOfWeekConverter = new ValueConverter<List<DayOfWeek>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<DayOfWeek>>(v, (JsonSerializerOptions?)null) ?? new List<DayOfWeek>());

        var daysOfWeekComparer = new ValueComparer<List<DayOfWeek>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        modelBuilder.Entity<Product>()
            .Property(p => p.DaysOfWeek)
            .HasConversion(daysOfWeekConverter)
            .Metadata.SetValueComparer(daysOfWeekComparer);

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
            Name = "Brownie",
            Price = 2.00,
            ImageURL = "https://static.itdg.com.br/images/640-400/0191a4f23349e54e618a65f2051d68a8/shutterstock-1915577575-2-.jpg"
        },
        new Product
        {
            Name = "Key Lime Cheesecake",
            Price = 8.00,
            ImageURL = "http://1.bp.blogspot.com/-7we9Z0C_fpI/T90JXcg3YsI/AAAAAAAABn4/EN7u2vMuRug/s1600/key+lime+cheesecake+slice+in+front.jpg",
            SpecificDate = new DateTime(DateTime.Now.Year, 10, 1) 
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

        Products.AddRange(products);
        SaveChanges();

        var cookie = Products.First(p => p.Name == "Cookie");
        var cheesecake = Products.First(p => p.Name == "Key Lime Cheesecake");
        var donut = Products.First(p => p.Name == "Mini Gingerbread Donut");
        var brownie = Products.First(p => p.Name == "Brownie");

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
            ProductId = cookie.Id,
            RequiredQuantity = 6,
            DiscountPercentage = 0.20,
            DiscountType = (int)DiscountType.Bulk
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