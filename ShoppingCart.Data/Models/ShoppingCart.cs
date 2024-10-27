using ShoppingCart.Core.Models;

namespace ShoppingCart.Data.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
