namespace ShoppingCart.Core.DTOs
{
    public class BulkPricingDto
    {
        public int Amount { get; set; } 
        public double TotalPrice { get; set; } // Precio total para la cantidad de volumen
    }
}