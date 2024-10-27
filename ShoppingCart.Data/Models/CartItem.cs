namespace ShoppingCart.Core.Models;

public class CartItem
{
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}