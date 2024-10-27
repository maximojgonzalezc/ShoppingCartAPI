using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Data;
using ShoppingCart.Data.MappingProfiles;
using ShoppingCart.Data.Repositories;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Configurar DbContext
        services.AddDbContext<ShoppingCartContext>(options =>
            options.UseSqlServer("Server=localhost,1433;Database=ShoppingCartDb;User Id=sa;Password=YourPassword123"));

        // Registrar el repositorio e interfaz
        services.AddScoped<IProductRepository, ProductRepository>();

        // Configurar AutoMapper
        services.AddAutoMapper(typeof(ProductProfile));
    });
