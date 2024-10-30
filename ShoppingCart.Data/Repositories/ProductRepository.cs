using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;
using ShoppingCart.Data.Contexts;

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
        return await _context.Products
            .Include(p => p.Discounts) 
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Discounts)
            .ToListAsync();
    }
}