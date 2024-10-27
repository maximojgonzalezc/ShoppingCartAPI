using ShoppingCart.Core.Models;

public class BulkPricing
{
    public int Id { get; set; }
    public int ProductId { get; set; } // Foreign key
    public required Product Product { get; set; } // Navigation property
    public int Amount { get; set; } // Cantidad mínima para aplicar precio por volumen
    public double TotalPrice { get; set; }
}