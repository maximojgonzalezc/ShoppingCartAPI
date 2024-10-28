using ShoppingCart.Core.DTOs;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly List<ShoppingCartItemDto> _items = new();
        private readonly IProductService _productService;

        public ShoppingCartService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task AddItem(int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) throw new ArgumentException("Invalid product ID");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _items.Add(new ShoppingCartItemDto { ProductId = productId, Quantity = quantity });
            }
        }

        public void ClearCart()
        {
            _items.Clear();
        }

        public double CalculateItemTotal(ProductDto product, int quantity, DateTime date)
        {
            double total = 0.0;

            // Verificar si es un día especial (día específico o día de la semana)
            bool isSpecialDay = (product.SpecificDate.HasValue && product.SpecificDate.Value.Date == date.Date) ||
                                product.DaysOfWeek.Contains(date.DayOfWeek);

            // Aplicar descuento por día especial si corresponde
            var specialDiscount = product.Discounts
                .FirstOrDefault(d => isSpecialDay && d.DiscountType == DiscountType.SpecialDay && d.RequiredQuantity <= quantity);

            if (specialDiscount != null)
            {
                int sets = quantity / specialDiscount.RequiredQuantity;
                total += specialDiscount.CalculateDiscount(product.Price, sets * specialDiscount.RequiredQuantity);
                quantity %= specialDiscount.RequiredQuantity; // Cantidad restante después del descuento especial
            }

            // Aplicar descuento por cantidad (bulk) si el producto soporta bulk pricing y queda cantidad
            if (product.SupportsBulkPricing && quantity > 0)
            {
                var bulkDiscount = product.Discounts
                    .FirstOrDefault(d => d.DiscountType == DiscountType.Bulk && d.RequiredQuantity <= quantity);

                if (bulkDiscount != null)
                {
                    int sets = quantity / bulkDiscount.RequiredQuantity;
                    total += bulkDiscount.CalculateDiscount(product.Price, sets * bulkDiscount.RequiredQuantity);
                    quantity %= bulkDiscount.RequiredQuantity; // Cantidad restante después del descuento por cantidad
                }
            }

            // Calcular el precio regular para la cantidad restante
            total += quantity * product.Price;
            return total;
        }





        public double CalculateTotal(DateTime date)
        {
            return _items.Sum(item =>
            {
                var product = _productService.GetProductByIdAsync(item.ProductId).Result;
                return product != null ? CalculateItemTotal(product, item.Quantity, date) : 0.0;
            });
        }

        public List<ShoppingCartItemDto> GetItems()
        {
            return _items;
        }
    }
}