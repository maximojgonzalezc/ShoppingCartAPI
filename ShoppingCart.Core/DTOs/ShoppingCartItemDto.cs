namespace ShoppingCart.Core.DTOs;

public class ShoppingCartItemDto
{
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}
