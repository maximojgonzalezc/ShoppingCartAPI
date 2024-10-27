using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Models;

public class Discount
{
    public int Id { get; set; }
    public int ProductId { get; set; } // Foreign key
    public Product? Product { get; set; } // Navigation property
    public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>(); // Cambiado a una colección
    public DateTime? SpecificDate { get; set; } // Fecha específica (por ejemplo, "1 de Octubre")
    public double DiscountPercentage { get; set; } // Porcentaje de descuento (25%)
    public int RequiredQuantity { get; set; } // Cantidad mínima requerida para aplicar el descuento
    public double DiscountPrice { get; set; } // Precio fijo (por ejemplo, $6.00 por 8 cookies)
}