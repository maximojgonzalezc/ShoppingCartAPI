using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShoppingCartContext _context;

    public ProductRepository(ShoppingCartContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
}