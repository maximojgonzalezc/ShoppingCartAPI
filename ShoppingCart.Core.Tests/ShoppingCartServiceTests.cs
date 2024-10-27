using FluentAssertions;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;
using ShoppingCart.Core.Services;

namespace ShoppingCart.Core.Tests
{
    [TestClass]
    public class ShoppingCartServiceTests
    {
        private IShoppingCartService? _cartService;

        [TestInitialize]
        public void Setup()
        {
            // Given - Initialize the shopping cart service before each test
            _cartService = new ShoppingCartService();
        }

        [TestMethod]
        public void AddItem_ShouldAddProductToCart()
        {
            var product = new Product { Id = 1, Name = "Brownie", Price = 2.00 };
            _cartService!.AddItem(product, 3);

            var items = _cartService.GetItems();
            items.Should().HaveCount(1, "because one item was added to the cart");
            items[0].Quantity.Should().Be(3, "because the added quantity was 3");
        }

        [TestMethod]
        public void CalculateTotal_NoDiscountsOrBulkPricing_ShouldReturnCorrectTotal()
        {
            _cartService!.AddItem(new Product { Id = 1, Name = "Brownie", Price = 2.00 }, 3);

            var total = _cartService.CalculateTotal(DateTime.Now);

            total.Should().Be(6.00, "because 3 brownies at 2.00 each equals 6.00");
        }

        [TestMethod]
        public void CalculateTotal_WithBulkPricing_ShouldApplyBulkPricingCorrectly()
        {
            _cartService!.AddItem(new Product
            {
                Id = 1,
                Name = "Cookie",
                Price = 1.25,
                BulkPricing = new BulkPricing { Amount = 4, TotalPrice = 7.00 }
            }, 8);

            var total = _cartService.CalculateTotal(DateTime.Now);

            total.Should().Be(14.00, "because 8 cookies with bulk pricing (2 sets of 4 at 7.00 each) equals 14.00");
        }

        [TestMethod]
        public void CalculateTotal_WithFridayDiscount_ShouldApplyFridayDiscount()
        {
            _cartService!.AddItem(new Product { Id = 1, Name = "Cookie", Price = 1.25 }, 8);

            var total = _cartService.CalculateTotal(new DateTime(2024, 10, 25)); // A Friday

            total.Should().Be(6.00, "because Friday discount for 8 cookies is 6.00");
        }

        [TestMethod]
        public void CalculateTotal_WithOctoberDiscount_ShouldApplyOctoberDiscount()
        {
            _cartService!.AddItem(new Product { Id = 2, Name = "Key Lime Cheesecake", Price = 8.00 }, 2);

            var total = _cartService.CalculateTotal(new DateTime(2024, 10, 1));

            total.Should().Be(12.00, "because 25% discount in October for cheesecakes makes the total 12.00");
        }

        [TestMethod]
        public void CalculateTotal_WithTuesdayDiscount_ShouldApplyTuesdayDiscount()
        {
            _cartService!.AddItem(new Product { Id = 3, Name = "Mini Gingerbread Donut", Price = 0.50 }, 4);

            var total = _cartService.CalculateTotal(new DateTime(2024, 10, 22)); // A Tuesday

            total.Should().Be(1.00, "because 2-for-1 discount on Tuesday makes the total 1.00");
        }

        [TestMethod]
        public void ClearCart_ShouldRemoveAllItemsFromCart()
        {
            _cartService!.AddItem(new Product { Id = 1, Name = "Brownie", Price = 2.00 }, 3);
            _cartService.ClearCart();

            _cartService.GetItems().Should().BeEmpty("because ClearCart was called");
        }
    }
}