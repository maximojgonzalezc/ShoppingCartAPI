using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Mappings;
using ShoppingCart.Core.Services;
using ShoppingCart.Data.Contexts;
using ShoppingCart.Data.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ShoppingCartContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        services.AddAutoMapper(typeof(ApplicationProfile));

    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    Console.WriteLine("Connection String: " + Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));

    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    dbContext.Database.Migrate(); 
    dbContext.SeedData();
}

host.Run();