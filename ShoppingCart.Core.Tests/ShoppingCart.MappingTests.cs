using System.Collections.Generic;
using System.Linq;
using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Core.Mappings;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Models;
using ShoppingCart.Common.Enums;

namespace ShoppingCart.Tests
{
    [TestClass]
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public void Given_Product_When_MappingToProductDto_Then_PropertiesShouldMatch()
        {
            // Given
            var product = new Product
            {
                Id = 1,
                Name = "Cookie",
                Price = 1.25,
                DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Friday },
                Discounts = new List<Discount>
                {
                    new Discount
                    {
                        Id = 1,
                        ProductId = 1,
                        RequiredQuantity = 8,
                        DiscountPercentage = 0.4,
                        DiscountType = (int)DiscountType.SpecialDay
                    }
                }
            };

            // When
            var productDto = _mapper.Map<ProductDto>(product);

            // Then
            productDto.Should().NotBeNull();
            productDto.Id.Should().Be(product.Id);
            productDto.Name.Should().Be(product.Name);
            productDto.Price.Should().Be(product.Price);
            productDto.DaysOfWeek.Should().BeEquivalentTo(product.DaysOfWeek);
            productDto.Discounts.Should().HaveCount(product.Discounts.Count);
            productDto.Discounts.First().DiscountPercentage.Should().Be(product.Discounts.First().DiscountPercentage);
        }

        [TestMethod]
        public void Given_Discount_When_MappingToDiscountDto_Then_PropertiesShouldMatch()
        {
            // Given
            var discount = new Discount
            {
                Id = 1,
                ProductId = 1,
                RequiredQuantity = 5,
                DiscountPercentage = 0.25,
                DiscountType = (int)DiscountType.Bulk
            };

            // When
            var discountDto = _mapper.Map<DiscountDto>(discount);

            // Then
            discountDto.Should().NotBeNull();
            discountDto.Id.Should().Be(discount.Id);
            discountDto.ProductId.Should().Be(discount.ProductId);
            discountDto.RequiredQuantity.Should().Be(discount.RequiredQuantity);
            discountDto.DiscountPercentage.Should().Be(discount.DiscountPercentage);
            discountDto.DiscountType.Should().Be((DiscountType)discount.DiscountType);
        }
    }
}
