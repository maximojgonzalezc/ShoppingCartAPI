//using ShoppingCart.Core.Models;

//public class DiscountService : IDiscountService
//{
//    public double ApplyDiscounts(ShoppingCartItem item, double currentTotal, DateTime date)
//    {
//        var product = item.Product;

//        // Apply specific discounts based on ProductId
//        if (date.DayOfWeek == DayOfWeek.Friday && product.Id == ProductIds.Cookie && item.Quantity >= 8)
//        {
//            return 6.0; // Special price for 8 cookies on Fridays
//        }
//        else if (date.Month == 10 && product.Id == ProductIds.KeyLimeCheesecake)
//        {
//            return currentTotal * 0.75; // 25% discount in October for Key Lime Cheesecake
//        }
//        else if (date.DayOfWeek == DayOfWeek.Tuesday && product.Id == ProductIds.MiniGingerbreadDonut && item.Quantity >= 2)
//        {
//            return currentTotal - (item.Quantity / 2) * product.Price; // 2-for-1 on Tuesdays for Mini Gingerbread Donut
//        }

//        return currentTotal; // No discount applied
//    }
//}
