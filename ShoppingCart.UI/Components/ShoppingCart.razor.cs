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
        // Precio total sin descuentos aplicados
        totalPriceWithoutDiscount = cartItems.Sum(i => i.Product.Price * i.Quantity);
        totalItems = cartItems.Sum(i => i.Quantity);

        // Calcular total y descuentos utilizando CalculateItemTotal
        totalPrice = cartItems.Sum(i => CalculateItemTotal(i.Product, i.Quantity, purchaseDate));

        // Calcular y asignar descuentos de forma acumulativa
        specialDayDiscount = CalculateSpecialDayDiscount();
        bulkDiscount = CalculateBulkDiscount();
    }

    private double CalculateItemTotal(ProductDto product, int quantity, DateTime date)
    {
        double total = 0.0;

        // Verificar si es un día especial
        bool isSpecialDay = (product.SpecificDate.HasValue && product.SpecificDate.Value.Date == date.Date) ||
                            product.DaysOfWeek.Contains(date.DayOfWeek);

        // Aplicar descuento de "Special Day" si aplica
        var specialDiscount = product.Discounts
            .FirstOrDefault(d => isSpecialDay && d.DiscountType == DiscountType.SpecialDay && d.RequiredQuantity <= quantity);

        if (specialDiscount != null)
        {
            int sets = quantity / specialDiscount.RequiredQuantity;
            total += sets * specialDiscount.RequiredQuantity * product.Price * (1 - specialDiscount.DiscountPercentage);
            quantity %= specialDiscount.RequiredQuantity;
        }

        // Aplicar descuento por cantidad (Bulk) a la cantidad restante
        var bulkDiscount = product.Discounts
            .FirstOrDefault(d => d.DiscountType == DiscountType.Bulk && d.RequiredQuantity <= quantity);

        if (bulkDiscount != null)
        {
            int sets = quantity / bulkDiscount.RequiredQuantity;
            total += sets * bulkDiscount.RequiredQuantity * product.Price * (1 - bulkDiscount.DiscountPercentage);
            quantity %= bulkDiscount.RequiredQuantity;
        }

        // Sumar el precio sin descuento para la cantidad restante
        total += quantity * product.Price;
        return total;
    }

    private double CalculateSpecialDayDiscount()
    {
        return cartItems.Sum(i =>
        {
            var product = i.Product;
            bool isSpecialDay = (product.SpecificDate.HasValue && product.SpecificDate.Value.Date == purchaseDate.Date) ||
                                product.DaysOfWeek.Contains(purchaseDate.DayOfWeek);
            var discount = product.Discounts
                .FirstOrDefault(d => isSpecialDay && d.DiscountType == DiscountType.SpecialDay && d.RequiredQuantity <= i.Quantity);
            if (discount != null)
            {
                int sets = i.Quantity / discount.RequiredQuantity;
                return sets * discount.RequiredQuantity * i.Product.Price * discount.DiscountPercentage;
            }
            return 0;
        });
    }

    private double CalculateBulkDiscount()
    {
        return cartItems.Sum(i =>
        {
            var product = i.Product;
            var discount = product.Discounts
                .FirstOrDefault(d => d.DiscountType == DiscountType.Bulk && d.RequiredQuantity <= i.Quantity);
            if (discount != null)
            {
                int sets = i.Quantity / discount.RequiredQuantity;
                return sets * discount.RequiredQuantity * i.Product.Price * discount.DiscountPercentage;
            }
            return 0;
        });
    }
}
