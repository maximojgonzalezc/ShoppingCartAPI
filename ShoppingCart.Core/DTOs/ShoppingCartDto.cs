namespace ShoppingCart.Core.DTOs;

public class ShoppingCartDto
{
    public DateTime Date { get; set; }
    public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
}