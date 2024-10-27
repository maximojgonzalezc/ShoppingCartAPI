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

    public CalculateCartTotal(ILogger<CalculateCartTotal> logger, IShoppingCartService cartService)
    {
        _logger = logger;
        _cartService = cartService;
    }

    [Function("CalculateCartTotal")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "cart/calculate")] HttpRequestData req)
    {
        _logger.LogInformation("Calculating cart total...");

        // Leer el contenido del request (JSON) y validar
        var requestBody = await req.ReadAsStringAsync();
        if (string.IsNullOrEmpty(requestBody))
        {
            var emptyRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            emptyRequestResponse.WriteString("Request body is empty.");
            return emptyRequestResponse;
        }

        // Configurar opciones de deserialización
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Deserializar el cuerpo del request
        var cartRequest = JsonSerializer.Deserialize<CartRequest>(requestBody, options);

        if (cartRequest == null || cartRequest.Items == null || cartRequest.Items.Count == 0)
        {
            var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            badRequestResponse.WriteString("Invalid cart request.");
            return badRequestResponse;
        }

        // Limpiar el carrito para la nueva solicitud
        _cartService.ClearCart();

        // Agregar los productos al carrito
        foreach (var item in cartRequest.Items)
        {
            if (item.Product == null)
            {
                var invalidProductResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                invalidProductResponse.WriteString("Product information is missing.");
                return invalidProductResponse;
            }
            _cartService.AddItem(item.Product, item.Quantity);
        }

        // Calcular el total
        var total = _cartService.CalculateTotal(cartRequest.Date);

        // Crear la respuesta con el total calculado
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new { Total = total });

        return response;
    }
}