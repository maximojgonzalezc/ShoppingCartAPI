namespace ShoppingCart.Core.DTOs
{
    public enum DiscountType
    {
        SpecialDay,
        Bulk
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
}