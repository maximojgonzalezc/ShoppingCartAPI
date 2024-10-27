using ShoppingCart.Core.Models;

public interface IDiscountService
{
    public double ApplyDiscounts(ShoppingCartItem item, double currentTotal, DateTime date);
}