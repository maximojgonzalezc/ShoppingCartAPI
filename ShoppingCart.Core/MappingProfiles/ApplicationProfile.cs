using AutoMapper;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Data.Models;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            // Mapeo para Product <-> ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.DaysOfWeek, opt => opt.MapFrom(src => src.DaysOfWeek))
                .ForMember(dest => dest.SpecificDate, opt => opt.MapFrom(src => src.SpecificDate))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.DaysOfWeek, opt => opt.MapFrom(src => src.DaysOfWeek))
                .ForMember(dest => dest.SpecificDate, opt => opt.MapFrom(src => src.SpecificDate))
                .ForMember(dest => dest.Discounts, opt => opt.MapFrom(src => src.Discounts));

            // Mapeo para Discount <-> DiscountDto
            CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (DiscountType)src.DiscountType))
                .ForMember(dest => dest.RequiredQuantity, opt => opt.MapFrom(src => src.RequiredQuantity))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage));

            CreateMap<DiscountDto, Discount>()
                .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (int)src.DiscountType))
                .ForMember(dest => dest.RequiredQuantity, opt => opt.MapFrom(src => src.RequiredQuantity))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage));

            // Mapeo para ShoppingCartItem <-> ShoppingCartItemDto
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<ShoppingCartItemDto, ShoppingCartItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}