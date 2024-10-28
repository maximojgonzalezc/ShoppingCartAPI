using ShoppingCart.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCart.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    }
}