using AutoMapper;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Data.Models;
using ShoppingCart.Common.Enums;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Core.Mappings;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        // Mapeo para Product <-> ProductDto
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();

        // Mapeo para Discount <-> DiscountDto con casting de DiscountType
        CreateMap<Discount, DiscountDto>()
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (DiscountType)src.DiscountType));

        CreateMap<DiscountDto, Discount>()
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (int)src.DiscountType));

        // Mapeo para ShoppingCartItem <-> ShoppingCartItemDto (AutoMapper infiere las propiedades)
        CreateMap<ShoppingCartItem, ShoppingCartItemDto>();
        CreateMap<ShoppingCartItemDto, ShoppingCartItem>();
    }
}