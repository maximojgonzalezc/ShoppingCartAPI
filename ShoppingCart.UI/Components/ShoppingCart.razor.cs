using System.Net.Http.Json;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using ShoppingCart.UI.Dtos;

namespace ShoppingCartUI.Components;

public partial class ShoppingCart : ComponentBase
{
    [Inject]
    private HttpClient Http { get; set; }

    [Inject] private IToastService ToastService { get; set; }

    private List<ProductDto> products = new List<ProductDto>();
    private List<CartItemDto> cartItems = new List<CartItemDto>();
    private int totalItems;
    private double totalPriceWithoutDiscount;
    private double totalPrice;
    private double specialDayDiscount;
    private double bulkDiscount;
    public DateTime purchaseDate = DateTime.Today;

    private string specialDayDiscountFormatted => specialDayDiscount.ToString("0.##");

    private string bulkDiscountFormatted => bulkDiscount.ToString("0.##");
    private string totalSavingsFormatted => (specialDayDiscount + bulkDiscount).ToString("0.##");
    private string finalPriceFormatted => totalPrice.ToString("0.##");

    protected override async Task OnInitializedAsync()
    {
        products = await Http.GetFromJsonAsync<List<ProductDto>>("api/products");
        cartItems = products.Select(p => new CartItemDto { Product = p, Quantity = 1 }).ToList();
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

    public void IncreaseQuantity(CartItemDto item)
    {
        item.Quantity++;
        UpdateCartSummary();
    }

    public void DecreaseQuantity(CartItemDto item)
    {
        if (item.Quantity > 1)
        {
            item.Quantity--;
            UpdateCartSummary();
        }
    }

    public void RemoveItem(CartItemDto item)
    {
        cartItems.Remove(item);
        UpdateCartSummary();
    }

    private void UpdateCartSummary()
    {
        totalItems = cartItems.Sum(i => i.Quantity);
        totalPriceWithoutDiscount = cartItems.Sum(i => i.Product.Price * i.Quantity);

        specialDayDiscount = CalculateSpecialDayDiscount();
        bulkDiscount = CalculateBulkDiscount(); 

        totalPrice = totalPriceWithoutDiscount - specialDayDiscount - bulkDiscount;
    }

    private double CalculateSpecialDayDiscount()
    {
        double specialDiscountTotal = 0;
        foreach (var item in cartItems)
        {
            var product = item.Product;

            bool isSpecialDay = (product.SpecificDate.HasValue && product.SpecificDate.Value.Date == purchaseDate.Date) ||
                                product.DaysOfWeek.Contains(purchaseDate.DayOfWeek);

            var discount = product.Discounts
                .FirstOrDefault(d => isSpecialDay && d.DiscountType == DiscountType.SpecialDay && d.RequiredQuantity <= item.Quantity);

            if (discount != null)
            {
                int specialSets = item.Quantity / discount.RequiredQuantity;
                specialDiscountTotal += specialSets * discount.RequiredQuantity * product.Price * discount.DiscountPercentage;

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
            var remainingQuantity = item.RemainingQuantityAfterSpecial; 

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

    public async Task ValidateCartTotalAsync()
    {
        var cartRequest = new ShoppingCartDto
        {
            Date = purchaseDate,
            Items = cartItems
                .Where(ci => ci.Quantity > 0)
                .Select(ci => new ShoppingCartItemDto
                {
                    ProductId = ci.Product.Id,
                    Quantity = ci.Quantity
                }).ToList()
        };

        var response = await Http.PostAsJsonAsync("api/cart/calculate", cartRequest);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<CartTotalResponse>();

            if (result != null && result.Total == totalPrice)
            {
                Console.WriteLine("Cart total validated successfully.");
                ToastService.ShowSuccess("Cart total validated successfully.");
            }
            else
            {
                Console.WriteLine("Validation failed: totals do not match.");
                ToastService.ShowWarning("Validation failed: totals do not match.");
            }
        }
        else
        {
            Console.WriteLine("Error validating cart total: " + response.ReasonPhrase);
            ToastService.ShowError("Validation failed: totals do not match.");
        }

        StateHasChanged();
    }
}