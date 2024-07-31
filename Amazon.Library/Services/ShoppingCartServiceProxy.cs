using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Library.Models;

namespace Amazon.Library.Services
{
    public class ShoppingCartServiceProxy
    {
        private static ShoppingCartServiceProxy? instance;
        private static object instanceLock = new object();

        private List<ShoppingCart> carts;
        public List<ShoppingCart> Carts
        {
            get
            {
                return carts;
            }
        }

        public ShoppingCart AddCart(ShoppingCart cart)
        {
            if(cart.Id == 0)
            {
                cart.Id = carts.Select(c => c.Id).Max() + 1;
            }
            carts.Add(cart);
            return cart;
        }

        private ShoppingCartServiceProxy()
        {
            carts = new List<ShoppingCart>() { new ShoppingCart() { Id = 1, Name = "My Cart" } };
        }

        public static ShoppingCartServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if(instance == null)
                    {
                        instance = new ShoppingCartServiceProxy();
                    }
                }
                return instance;
            }
        }
    }
}
