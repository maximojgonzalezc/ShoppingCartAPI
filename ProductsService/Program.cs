using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Mappings;
using ShoppingCart.Core.Services;
using ShoppingCart.Data.Contexts;
using ShoppingCart.Data.Repositories;
using ShoppingCart.Data;
using Microsoft.Data.Sqlite;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Opction, i used Microsoft SQL Server, but for testing purposes ill ad an in-memory database

        //services.AddDbContext<ShoppingCartContext>(options =>
        //{
        //    var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        //    options.UseSqlServer(connectionString);
        //});

        services.AddDbContext();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IShoppingCartService, ShoppingCartService>();

        services.AddAutoMapper(typeof(ApplicationProfile));

    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    dbContext.Database.EnsureCreated(); // Crea la base de datos
    //dbContext.Database.Migrate(); // Aplica migraciones
    dbContext.SeedData();
}

// Al final del programa, cierra la conexión SQLite
var connection = host.Services.GetRequiredService<SqliteConnection>();
connection.Close();

host.Run();

using (var scope = host.Services.CreateScope())
{
    Console.WriteLine("Connection String: " + Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"));

    var dbContext = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();
    dbContext.Database.Migrate(); 
    dbContext.SeedData();
}

host.Run();