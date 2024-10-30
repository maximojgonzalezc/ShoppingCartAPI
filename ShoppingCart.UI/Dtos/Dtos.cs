namespace ShoppingCart.UI.Dtos;

public class CartItemDto
{
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice => Quantity * Product.Price;
    public int RemainingQuantityAfterSpecial { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public List<DiscountDto> Discounts { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();
    public DateTime? SpecificDate { get; set; }
    public string ImageURL { get; set; }
}

public class DiscountDto
{
    public int RequiredQuantity { get; set; }
    public double DiscountPercentage { get; set; }
    public DiscountType DiscountType { get; set; }

    public double CalculateDiscount(double basePrice, int quantity)
    {
        return quantity * basePrice * (1 - DiscountPercentage);
    }
}

public class ShoppingCartItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ShoppingCartDto
{
    public DateTime Date { get; set; }
    public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
}

public enum DiscountType
{
    SpecialDay,
    Bulk
}

public class CartTotalResponse
{
    public double Total { get; set; }
}
