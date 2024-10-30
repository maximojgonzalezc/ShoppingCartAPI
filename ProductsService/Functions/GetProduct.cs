using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Interfaces;
using ShoppingCart.Core.DTOs;
using System.Threading.Tasks;

namespace ProductsService.Functions
{
    public class GetProduct
    {
        private readonly ILogger<GetProduct> _logger;
        private readonly IProductService _productService;

        public GetProduct(ILogger<GetProduct> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Function("GetProduct")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "products/{id}")] HttpRequestData req,
            string id)
        {
            _logger.LogInformation($"Fetching product with ID {id}");

            if (!int.TryParse(id, out int productId))
            {
                var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                badRequestResponse.WriteString("Invalid product ID format.");
                return badRequestResponse;
            }

            var productDto = await _productService.GetProductByIdAsync(productId);

            if (productDto == null)
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
                notFoundResponse.WriteString($"Product with ID {id} not found.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);

            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

            await response.WriteAsJsonAsync(productDto);

            return response;
        }
    }
}
