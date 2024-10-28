using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Models;

public class Discount
{
    public double DiscountPercentage { get; set; } // Porcentaje de descuento (ej: 0.40 para 40%)
    public int RequiredQuantity { get; set; } // Cantidad mínima para que se aplique el descuento
}