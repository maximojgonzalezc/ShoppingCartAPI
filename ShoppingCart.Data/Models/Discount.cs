using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Models;

public class Discount
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int RequiredQuantity { get; set; } 
    public double? DiscountPercentage { get; set; } 
    public int DiscountType { get; set; }
}