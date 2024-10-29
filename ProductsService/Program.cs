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
        // Configuración del DbContext
        services.AddDbContext<ShoppingCartContext>(options =>
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            options.UseSqlServer(connectionString);
        });

        // Repositorios
        services.AddScoped<IProductRepository, ProductRepository>();

        // Servicios de aplicación
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        // AutoMapper
        services.AddAutoMapper(typeof(ApplicationProfile));

        // Otros servicios adicionales...
    })
    .Build();

// Ejecutar migraciones y cargar datos iniciales
using (var scope = host.Services.CreateScope())
{
    Console.WriteLine("Connection String: " + Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));

    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    dbContext.Database.Migrate(); // Asegura que las migraciones están aplicadas
    dbContext.SeedData();         // Invoca SeedData para insertar datos iniciales
}

host.Run();