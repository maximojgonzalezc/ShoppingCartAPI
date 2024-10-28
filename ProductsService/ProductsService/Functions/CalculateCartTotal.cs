using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.Models;

namespace ProductsService.Functions;

public class CalculateCartTotal
{
    private readonly ILogger<CalculateCartTotal> _logger;
    private readonly IShoppingCartService _cartService;
    private readonly IProductService _productService; // Inyectar el servicio de productos

    public CalculateCartTotal(ILogger<CalculateCartTotal> logger, IShoppingCartService cartService, IProductService productService)
    {
        _logger = logger;
        _cartService = cartService;
        _productService = productService;
    }

    [Function("CalculateCartTotal")]
    public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "cart/calculate")] HttpRequestData req)
    {
        _logger.LogInformation("Calculating cart total...");

        var requestBody = await req.ReadAsStringAsync();
        if (string.IsNullOrEmpty(requestBody))
        {
            var emptyRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            emptyRequestResponse.WriteString("Request body is empty.");
            return emptyRequestResponse;
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var cartRequest = JsonSerializer.Deserialize<CartRequest>(requestBody, options);

        if (cartRequest == null || cartRequest.Items == null || cartRequest.Items.Count == 0)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            badRequestResponse.WriteString("Invalid cart request.");
            return badRequestResponse;
        }

        _cartService.ClearCart();

        foreach (var item in cartRequest.Items)
        {
            if (item.ProductId == 0)
            {
                var invalidProductResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                invalidProductResponse.WriteString("Product ID is missing.");
                return invalidProductResponse;
            }

            // Obtener el producto desde el servicio
            var product = await _productService.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                notFoundResponse.WriteString($"Product with ID {item.ProductId} not found.");
                return notFoundResponse;
            }

            await _cartService.AddItem(product.Id, item.Quantity);
        }

        var total = _cartService.CalculateTotal(cartRequest.Date);
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { Total = total });

        return response;
    }

}