using ShoppingCart.Data.Models;

namespace ShoppingCart.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageURL { get; set; }
    public double Price { get; set; }
    public List<Discount> Discounts { get; set; } = new List<Discount>();
    public bool SupportsBulkPricing { get; set; } // Bandera para indicar si soporta Bulk Pricing
    public List<DayOfWeek> DaysOfWeek { get; set; } = new(); // Días aplicables semanalmente
    public DateTime? SpecificDate { get; set; } // Día específico (e.g., 1 de octubre)

}
