namespace ShoppingCart.Core.Models;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public Data.Models.ShoppingCart? ShoppingCart { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; } // Propiedad de navegación

    public int Quantity { get; set; }
    public double? DiscountedPrice { get; set; } // Precio final tras aplicar descuentos
}