using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Services;
using ShoppingCart.Data;
using ShoppingCart.Data.MappingProfiles;
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
        services.AddAutoMapper(typeof(ProductProfile));

        // Otros servicios adicionales...
    })
    .Build();

// Ejecutar migraciones y cargar datos iniciales
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    dbContext.Database.Migrate(); // Asegura que las migraciones están aplicadas
    dbContext.SeedData();         // Invoca SeedData para insertar datos iniciales
}

host.Run();
