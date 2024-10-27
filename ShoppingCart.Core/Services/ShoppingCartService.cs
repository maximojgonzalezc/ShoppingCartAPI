using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly List<ShoppingCartItem> _items = new();

        public void AddItem(Product product, int quantity)
        {
            var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new ShoppingCartItem { Product = product, Quantity = quantity });
            }
        }

        public void ClearCart()
        {
            _items.Clear();
        }

        public double CalculateTotal(DateTime date)
        {
            double total = 0.0;

            foreach (var item in _items)
            {
                total += CalculateItemTotal(item, date);
            }

            return total;
        }

        private double CalculateItemTotal(ShoppingCartItem item, DateTime date)
        {
            double total = 0.0;
            var product = item.Product;

            // Aplicar precios por volumen
            if (product.BulkPricing != null && item.Quantity >= product.BulkPricing.Amount)
            {
                int bulkSets = item.Quantity / product.BulkPricing.Amount;
                int remainder = item.Quantity % product.BulkPricing.Amount;
                total += bulkSets * product.BulkPricing.TotalPrice + remainder * product.Price;
            }
            else
            {
                // Precio regular
                total += item.Quantity * product.Price;
            }

            // Aplicar descuentos basados en la fecha o día
            if (date.DayOfWeek == DayOfWeek.Friday && product.Name == "Cookie" && item.Quantity >= 8)
            {
                total = 6.0; // Precio especial para 8 cookies los viernes
            }
            else if (date.Month == 10 && product.Name == "Key Lime Cheesecake")
            {
                total *= 0.75; // 25% de descuento en octubre para "Key Lime Cheesecake"
            }
            else if (date.DayOfWeek == DayOfWeek.Tuesday && product.Name == "Mini Gingerbread Donut" && item.Quantity >= 2)
            {
                total -= (item.Quantity / 2) * product.Price; // 2x1 en martes para "Mini Gingerbread Donut"
            }

            return total;
        }

        public List<ShoppingCartItem> GetItems()
        {
            return _items;
        }
    }
}