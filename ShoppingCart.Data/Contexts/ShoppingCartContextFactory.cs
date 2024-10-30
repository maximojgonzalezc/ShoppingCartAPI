using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShoppingCart.Data.Contexts
{
    public class ShoppingCartContextFactory : IDesignTimeDbContextFactory<ShoppingCartContext>
    {
        public ShoppingCartContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShoppingCartContext>();
            optionsBuilder.UseSqlite("Data Source=ShoppingCartDb;Mode=memory;Cache=shared");

            return new ShoppingCartContext(optionsBuilder.Options);
        }
    }
}
