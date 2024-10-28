
using ShoppingCart.Core.DTOs;

namespace ShoppingCart.Core.Interfaces
{
    public interface IShoppingCartService
    {
        public Task AddItem (int productId, int quantity);

        public void ClearCart();

        double CalculateItemTotal(ProductDto product, int quantity, DateTime date);

        public List<ShoppingCartItemDto> GetItems();

        double CalculateTotal(DateTime date);
    }
} 