using ShoppingCart.Data.Models;

namespace ShoppingCart.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageURL { get; set; }
    public double Price { get; set; }
    public List<Discount> Discounts { get; set; } = new List<Discount>();
    public List<DayOfWeek>? DaysOfWeek { get; set; } = new();
    public DateTime? SpecificDate { get; set; } 
}