using FluentAssertions;
using Moq;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Services;

namespace ShoppingCart.Core.Tests;

[TestClass]
public class ShoppingCartServiceTests
{
    private IShoppingCartService _cartService;
    private Mock<IProductService> _productServiceMock;

    [TestInitialize]
    public void Setup()
    {
        _productServiceMock = new Mock<IProductService>();
        _cartService = new ShoppingCartService(_productServiceMock.Object);
    }

    [TestMethod]
    public async Task CalculateTotal_NoSales_EightCookies_ShouldReturnCorrectTotal()
    {
        var cookie = new ProductDto
        {
            Id = 1,
            Name = "Cookie",
            Price = 1.25,
            SupportsBulkPricing = true,
            Discounts = new List<DiscountDto>
            {
                new DiscountDto { RequiredQuantity = 6, DiscountPercentage = 0.20, DiscountType = DiscountType.Bulk }
            }
        };

        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cookie.Id)).ReturnsAsync(cookie);

        await _cartService.AddItem(cookie.Id, 8);
        var total = _cartService.CalculateTotal(new DateTime(2024, 10, 30));

        total.Should().Be(8.50, "because 8 cookies without sales total $8.50");
    }

    [TestMethod]
    public async Task CalculateTotal_NoSales_MixedItems_ShouldReturnCorrectTotal()
    {
        var cookie = new ProductDto
        {
            Id = 1,
            Name = "Cookie",
            Price = 1.25,
            SupportsBulkPricing = true,
            Discounts = new List<DiscountDto> { new DiscountDto { RequiredQuantity = 6, DiscountPercentage = 0.20, DiscountType = DiscountType.Bulk } }
        };
        var brownie = new ProductDto { Id = 2, Name = "Brownie", Price = 2.00 };
        var cheesecake = new ProductDto { Id = 3, Name = "Key Lime Cheesecake", Price = 8.00 };
        var donut = new ProductDto
        {
            Id = 4,
            Name = "Mini Gingerbread Donut",
            Price = 0.50,
            Discounts = new List<DiscountDto> { new DiscountDto { RequiredQuantity = 2, DiscountPercentage = 0.50, DiscountType = DiscountType.SpecialDay } },
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Tuesday } // Descuento aplicable solo los martes
        };

        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cookie.Id)).ReturnsAsync(cookie);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(brownie.Id)).ReturnsAsync(brownie);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cheesecake.Id)).ReturnsAsync(cheesecake);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(donut.Id)).ReturnsAsync(donut);

        await _cartService.AddItem(cookie.Id, 1);
        await _cartService.AddItem(brownie.Id, 1);
        await _cartService.AddItem(cheesecake.Id, 1);
        await _cartService.AddItem(donut.Id, 2);

        // Utiliza un día que no sea martes (por ejemplo, un miércoles) para evitar el descuento
        var fixedDate = new DateTime(2024, 10, 30); // Un miércoles
        var total = _cartService.CalculateTotal(fixedDate);

        total.Should().Be(12.25, "because 1 cookie, 1 brownie, 1 cheesecake, and 2 donuts without sales total $12.25");
    }


    [TestMethod]
    public async Task CalculateTotal_NoSales_OneCookieFourBrowniesOneCheesecake_ShouldReturnCorrectTotal()
    {
        var cookie = new ProductDto
        {
            Id = 1,
            Name = "Cookie",
            Price = 1.25,
            SupportsBulkPricing = true,
            Discounts = new List<DiscountDto> { new DiscountDto { RequiredQuantity = 6, DiscountPercentage = 0.20, DiscountType = DiscountType.Bulk } }
        };
        var brownie = new ProductDto
        {
            Id = 2,
            Name = "Brownie",
            Price = 2.00,
            SupportsBulkPricing = true,
            Discounts = new List<DiscountDto> { new DiscountDto { RequiredQuantity = 4, DiscountPercentage = 0.125, DiscountType = DiscountType.Bulk } }
        };
        var cheesecake = new ProductDto
        {
            Id = 3,
            Name = "Key Lime Cheesecake",
            Price = 8.00
        };

        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cookie.Id)).ReturnsAsync(cookie);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(brownie.Id)).ReturnsAsync(brownie);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cheesecake.Id)).ReturnsAsync(cheesecake);

        await _cartService.AddItem(cookie.Id, 1);
        await _cartService.AddItem(brownie.Id, 4);
        await _cartService.AddItem(cheesecake.Id, 1);

        var total = _cartService.CalculateTotal(new DateTime(2024, 10, 30));
        total.Should().Be(16.25, "because 1 cookie, 4 brownies, and 1 cheesecake without sales total $16.25");
    }

    [TestMethod]
    public async Task CalculateTotal_SpecialDay_OctoberFirst_EightCookiesFourCheesecakes_ShouldReturnCorrectTotal()
    {
        var cookie = new ProductDto
        {
            Id = 1,
            Name = "Cookie",
            Price = 1.25,
            SupportsBulkPricing = true,
            Discounts = new List<DiscountDto>
            {
                new DiscountDto { RequiredQuantity = 8, DiscountPercentage = 0.40, DiscountType = DiscountType.SpecialDay },
                new DiscountDto { RequiredQuantity = 6, DiscountPercentage = 0.20, DiscountType = DiscountType.Bulk }
            },
            DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday }
        };

        var cheesecake = new ProductDto
        {
            Id = 2,
            Name = "Key Lime Cheesecake",
            Price = 8.00,
            Discounts = new List<DiscountDto>
            {
                new DiscountDto { DiscountPercentage = 0.25, RequiredQuantity = 1, DiscountType = DiscountType.SpecialDay }
            },
            SpecificDate = new DateTime(2024, 10, 1)
        };

        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cookie.Id)).ReturnsAsync(cookie);
        _productServiceMock.Setup(ps => ps.GetProductByIdAsync(cheesecake.Id)).ReturnsAsync(cheesecake);

        await _cartService.AddItem(cookie.Id, 8);
        await _cartService.AddItem(cheesecake.Id, 4);

        var total = _cartService.CalculateTotal(new DateTime(2024, 10, 1));
        total.Should().Be(32.50, "because on October 1st, 8 cookies and 4 cheesecakes with a 25% discount total $32.50");
    }
}