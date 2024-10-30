using ShoppingCart.Common.Enums;

namespace ShoppingCart.Core.DTOs
{
    public class DiscountDto
    {
        public int Id { get; set; } 
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
        public double DiscountPercentage { get; set; }
        public DiscountType DiscountType { get; set; }

        public double CalculateDiscount(double basePrice, int quantity)
        {
            return quantity * basePrice * (1 - DiscountPercentage);
        }
    }
}
