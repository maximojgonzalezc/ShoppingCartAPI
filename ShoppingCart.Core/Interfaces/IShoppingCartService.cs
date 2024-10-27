
using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.Interfaces
{
    public interface IShoppingCartService
    {
        void AddItem(Product product, int quantity);

        void ClearCart();

        double CalculateTotal(DateTime date);

        List<ShoppingCartItem> GetItems();
    }
}