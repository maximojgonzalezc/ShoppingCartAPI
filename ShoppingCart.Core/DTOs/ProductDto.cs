using ShoppingCart.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public List<DiscountDto> Discounts { get; set; } = new();
    public List<DayOfWeek> DaysOfWeek { get; set; } = new(); // Días aplicables semanalmente
    public DateTime? SpecificDate { get; set; } // Día específico (e.g., 1 de octubre)
    public string? ImageURL { get; set; }
}