using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using ShoppingCart.UI.Dtos;

namespace ShoppingCartUI.Components;

public partial class ShoppingCart : ComponentBase
{
    [Inject]
    private HttpClient Http { get; set; }

    private List<ProductDto> products = new List<ProductDto>();
    private List<CartItem> cartItems = new List<CartItem>();
    private int totalItems;
    private double totalPriceWithoutDiscount;
    private double totalPrice;
    private double specialDayDiscount;
    private double bulkDiscount;
    public DateTime purchaseDate = DateTime.Today;

    // Formato para mostrar descuentos y precios
    private string specialDayDiscountFormatted => specialDayDiscount.ToString("0.##");
    private string bulkDiscountFormatted => bulkDiscount.ToString("0.##");
    private string totalSavingsFormatted => (specialDayDiscount + bulkDiscount).ToString("0.##");
    private string finalPriceFormatted => totalPrice.ToString("0.##");

    protected override async Task OnInitializedAsync()
    {
        products = await Http.GetFromJsonAsync<List<ProductDto>>("api/products");
        cartItems = products.Select(p => new CartItem { Product = p, Quantity = 1 }).ToList();
        UpdateCartSummary();
    }

    public void OnDateChanged(ChangeEventArgs e)
    {
        if (DateTime.TryParse(e.Value?.ToString(), out var date))
        {
            purchaseDate = date;
            UpdateCartSummary();
        }
    }

    public void IncreaseQuantity(CartItem item)
    {
        item.Quantity++;
        UpdateCartSummary();
    }

    public void DecreaseQuantity(CartItem item)
    {
        if (item.Quantity > 1)
        {
            item.Quantity--;
            UpdateCartSummary();
        }
    }

    public void RemoveItem(CartItem item)
    {
        cartItems.Remove(item);
        UpdateCartSummary();
    }

    private void UpdateCartSummary()
    {
        totalItems = cartItems.Sum(i => i.Quantity);
        totalPriceWithoutDiscount = cartItems.Sum(i => i.Product.Price * i.Quantity);

        // Reiniciar descuentos para recalcular
        specialDayDiscount = CalculateSpecialDayDiscount();
        bulkDiscount = CalculateBulkDiscount();  // Solo aplica si no hubo descuento de "special day" completo

        // Calcular el precio total final con descuentos aplicados
        totalPrice = totalPriceWithoutDiscount - specialDayDiscount - bulkDiscount;
    }

    private double CalculateSpecialDayDiscount()
    {
        double specialDiscountTotal = 0;
        foreach (var item in cartItems)
        {
            var product = item.Product;

            // Comprobar si aplica descuento "special day" para este producto
            bool isSpecialDay = (product.SpecificDate.HasValue && product.SpecificDate.Value.Date == purchaseDate.Date) ||
                                product.DaysOfWeek.Contains(purchaseDate.DayOfWeek);

            var discount = product.Discounts
                .FirstOrDefault(d => isSpecialDay && d.DiscountType == DiscountType.SpecialDay && d.RequiredQuantity <= item.Quantity);

            if (discount != null)
            {
                int specialSets = item.Quantity / discount.RequiredQuantity;
                specialDiscountTotal += specialSets * discount.RequiredQuantity * product.Price * discount.DiscountPercentage;

                // Ajustar cantidad restante después del "special day" para posibles "bulk discounts"
                item.RemainingQuantityAfterSpecial = item.Quantity % discount.RequiredQuantity;
            }
            else
            {
                item.RemainingQuantityAfterSpecial = item.Quantity;
            }
        }
        return specialDiscountTotal;
    }

    private double CalculateBulkDiscount()
    {
        double bulkDiscountTotal = 0;
        foreach (var item in cartItems)
        {
            var product = item.Product;
            var remainingQuantity = item.RemainingQuantityAfterSpecial;  // Cantidad restante para aplicar "bulk discount"

            var discount = product.Discounts
                .FirstOrDefault(d => d.DiscountType == DiscountType.Bulk && d.RequiredQuantity <= remainingQuantity);

            if (discount != null)
            {
                int bulkSets = remainingQuantity / discount.RequiredQuantity;
                bulkDiscountTotal += bulkSets * discount.RequiredQuantity * product.Price * discount.DiscountPercentage;
            }
        }
        return bulkDiscountTotal;
    }
}
