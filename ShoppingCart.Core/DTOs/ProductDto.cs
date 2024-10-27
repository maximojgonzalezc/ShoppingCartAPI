namespace ShoppingCart.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public BulkPricingDto? BulkPricing { get; set; }
}