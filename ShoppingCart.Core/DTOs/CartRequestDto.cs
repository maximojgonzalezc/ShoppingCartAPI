namespace ShoppingCart.Core.DTOs;

public class CartRequestDto
{
    public List<CartItemDto> Items { get; set; } = new();
    public DateTime Date { get; set; } = DateTime.Now;
}
