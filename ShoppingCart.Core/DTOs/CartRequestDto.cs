using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.DTOs;

public class CartRequestDto
{
    public List<ShoppingCartItemDto> Items { get; set; } = new();
    public DateTime Date { get; set; } = DateTime.Now;
}
