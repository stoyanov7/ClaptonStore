namespace ClaptonStore.Controllers
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Web;

    public class ShoppingCartController : Controller
    {
        private readonly ClaptonStoreContext productRepository;
        private readonly ShoppingCart cart;

        public ShoppingCartController(ClaptonStoreContext productRepository, ShoppingCart shoppingCart)
        {
            this.productRepository = productRepository;
            cart = shoppingCart;
        }
        
        public IActionResult Index()
        {
            var items = cart.GetShoppingCartItems();
            cart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = cart,
                ShoppingCartTotal = cart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }
        
        public async Task<IActionResult> AddToShoppingCart(int productId)
        {
            var product = await productRepository.Games.FirstOrDefaultAsync(p => p.Id == productId);

            if (product != null)
            {
                cart.AddToCart(product, 1);
            }
            return RedirectToAction("Index","Shop",null);
        }

        public async Task<IActionResult> RemoveFromShoppingCart(int productId)
        {
            var product =  await productRepository.Games.FirstOrDefaultAsync(p => p.Id == productId);
            if (product != null)
            {
                cart.RemoveFromCart(product);
            }
            return RedirectToAction("Index");
        }
    }
}