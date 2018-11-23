namespace ClaptonStore.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    public class ShoppingCart
    {
        private const string SessionKey = "CartId";
        private readonly ClaptonStoreContext context;

        private ShoppingCart(ClaptonStoreContext context)
        {
            this.context = context;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<ClaptonStoreContext>();
            string cartId = session.GetString(SessionKey) ?? Guid.NewGuid().ToString();

            session.SetString(SessionKey, cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(Game product, int amount)
        {
            var shoppingCartItem =
                    context.ShoppingCartItems.SingleOrDefault(
                        s => s.Product.Id  == product.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Amount = 1
                };

                context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            context.SaveChanges();
        }

        public int RemoveFromCart(Game product)
        {
            var shoppingCartItem =
                    context.ShoppingCartItems.SingleOrDefault(
                        s => s.Product.Id == product.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            context.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = context
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            context.ShoppingCartItems.RemoveRange(cartItems);

            context.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = context.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Amount).Sum();
            return total;
        }


    }
}
