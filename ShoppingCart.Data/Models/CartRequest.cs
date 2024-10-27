namespace ShoppingCart.Core.Models;

public class CartRequest
{
    public List<CartItem> Items { get; set; } = new();
    public DateTime Date { get; set; } = DateTime.Now;
}