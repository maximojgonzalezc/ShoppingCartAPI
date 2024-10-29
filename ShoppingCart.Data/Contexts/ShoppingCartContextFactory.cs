using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ShoppingCart.Data.Contexts;

public class ShoppingCartContextFactory : IDesignTimeDbContextFactory<ShoppingCartContext>
{
    public ShoppingCartContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShoppingCartContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ShoppingCartDb;Trusted_Connection=True;");

        return new ShoppingCartContext(optionsBuilder.Options);
    }
}
