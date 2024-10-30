using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Data.Contexts;

namespace ShoppingCart.Data;

public static class ServiceRegistration
{
    public static void AddDbContext(this IServiceCollection services)
    {
        var connection = new SqliteConnection("Data Source=ShoppingCartDb;Mode=memory;Cache=shared");
        connection.Open();

        services.AddDbContext<ShoppingCartContext>(
            options =>
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseSqlite(connection); 
            }, ServiceLifetime.Singleton
        );

        services.AddSingleton(connection);
    }
}