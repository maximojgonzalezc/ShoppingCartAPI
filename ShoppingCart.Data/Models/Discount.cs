using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Models;

public class Discount
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int RequiredQuantity { get; set; } // Cantidad mínima para aplicar el descuento
    public double? DiscountPrice { get; set; } // Precio total cuando se cumple con el requisito de cantidad o día
    public double? DiscountPercentage { get; set; } // Porcentaje de descuento para día especial
    public int DiscountType { get; set; } // Tipo de descuento (SpecialDay o Bulk)
    public List<DayOfWeek> DaysOfWeek { get; set; } = new(); // Días aplicables semanalmente
}