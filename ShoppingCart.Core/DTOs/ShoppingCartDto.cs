namespace ShoppingCart.Core.DTOs;

public class ShoppingCartDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
}