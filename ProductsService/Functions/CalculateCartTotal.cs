using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.DTOs;
using ShoppingCart.Core.Interfaces;

namespace ProductsService.Functions;

public class CalculateCartTotal
{
    private readonly ILogger<CalculateCartTotal> _logger;
    private readonly IShoppingCartService _cartService;
    private readonly IProductService _productService;

    public CalculateCartTotal(ILogger<CalculateCartTotal> logger, IShoppingCartService cartService, IProductService productService)
    {
        _logger = logger;
        _cartService = cartService;
        _productService = productService;
    }

    [Function("CalculateCartTotal")]
    public async Task<HttpResponseData> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "cart/calculate")] HttpRequestData req)
    {
        if (req.Method == HttpMethod.Options.Method)
        {
            var preflightResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return preflightResponse;
        }

        _logger.LogInformation("Calculating cart total...");

        var requestBody = await req.ReadAsStringAsync();
        if (string.IsNullOrEmpty(requestBody))
        {
            var emptyRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            emptyRequestResponse.WriteString("Request body is empty.");
            return emptyRequestResponse;
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var cartRequest = JsonSerializer.Deserialize<ShoppingCartDto>(requestBody, options);

        if (cartRequest == null || cartRequest.Items == null || cartRequest.Items.Count == 0)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            badRequestResponse.WriteString("Invalid cart request.");
            return badRequestResponse;
        }

        _cartService.ClearCart();

        foreach (var item in cartRequest.Items)
        {
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

        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
        response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

        await response.WriteAsJsonAsync(new { Total = total });

        return response;
    }
}