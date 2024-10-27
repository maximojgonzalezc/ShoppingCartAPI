using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Models;

namespace ProductsService.Functions;

public class GetProduct
{
    private readonly ILogger<GetProduct> _logger;

    public GetProduct(ILogger<GetProduct> logger)
    {
        _logger = logger;
    }

    [Function("GetProduct")]
    public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequestData req,
    string id)
    {
        _logger.LogInformation($"Fetching product with ID {id}");

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "ProductsService", "products.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var products = JsonSerializer.Deserialize<List<Product>>(await File.ReadAllTextAsync(jsonPath), options);

        if (int.TryParse(id, out int productId))
        {
            var product = products?.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                notFoundResponse.WriteString($"Product with ID {id} not found.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(product);

            return response;
        }
        else
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            badRequestResponse.WriteString("Invalid product ID format.");
            return badRequestResponse;
        }
    }

}