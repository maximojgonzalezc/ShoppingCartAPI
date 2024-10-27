namespace ShoppingCart.Core.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImageURL { get; set; }
    public double Price { get; set; }
    public BulkPricing? BulkPricing { get; set; }
}