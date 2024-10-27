namespace ShoppingCart.Core.Models;

public class ShoppingCartItem
{
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}