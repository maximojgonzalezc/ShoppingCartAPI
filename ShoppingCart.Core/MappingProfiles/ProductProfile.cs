using AutoMapper;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Mapeo de Product a ProductDto y viceversa
        CreateMap<Product, ProductDto>().ReverseMap();

        // Mapeo de BulkPricing a BulkPricingDto y viceversa (si es necesario)
        CreateMap<BulkPricing, BulkPricingDto>().ReverseMap();
    }
}