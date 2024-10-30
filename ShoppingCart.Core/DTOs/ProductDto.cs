using ShoppingCart.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public List<DiscountDto> Discounts { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new();
    public DateTime? SpecificDate { get; set; } 
    public string? ImageURL { get; set; }
}