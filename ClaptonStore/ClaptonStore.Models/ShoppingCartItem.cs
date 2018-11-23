
namespace ClaptonStore.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }

        public Game Product { get; set; }

        public int Amount { get; set; } 

        public string ShoppingCartId { get; set; }
    }
}
