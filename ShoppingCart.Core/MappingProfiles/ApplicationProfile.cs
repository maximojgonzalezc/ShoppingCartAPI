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
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();

        CreateMap<Discount, DiscountDto>()
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (DiscountType)src.DiscountType));

        CreateMap<DiscountDto, Discount>()
            .ForMember(dest => dest.DiscountType, opt => opt.MapFrom(src => (int)src.DiscountType));

    }
}