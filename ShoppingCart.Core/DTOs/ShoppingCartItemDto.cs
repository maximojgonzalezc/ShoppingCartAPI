using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.DTOs;

public class ShoppingCartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}