using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Library.Models;
using System.Collections.ObjectModel;

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

        private ShoppingCartServiceProxy()
        {
            carts = new List<ShoppingCart>() { new ShoppingCart { Id = 1, Name = "My Cart" } };
            taxRate = 7m;
        }

        private decimal taxRate;
        public decimal TaxRate
        {
            get
            {
                return taxRate;
            }
            set
            {
                taxRate = value;
            }
        }
        public decimal TotalPrice(ShoppingCart cart)
        {
                return cart.Contents.Sum(p => p.Price * p.Quantity);
        }
        public decimal CalculateTotalPriceWithTax(ShoppingCart cart)
        {
            decimal totalPrice = TotalPrice(cart);
            decimal taxAmount = totalPrice * TaxRate;
            return totalPrice + taxAmount;
        }

        public ShoppingCart AddCart(ShoppingCart cart)
        {
            if (cart.Id == 0)
            {
                cart.Id = carts.Select(c => c.Id).Max() + 1;
            }

            carts.Add(cart);

            return cart;
        }

        public static ShoppingCartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ShoppingCartServiceProxy();
                    }
                }
                return instance;
            }
        }

        public void AddToCart(Product newProduct, int id)
        {
            var cartToUse = Carts.FirstOrDefault(c => c.Id == id);
            if (cartToUse == null || cartToUse.Contents == null)
            {
                return;
            }

            var existingProduct = cartToUse?.Contents?
                .FirstOrDefault(existingProducts => existingProducts.Id == newProduct.Id);

            var inventoryProduct = InventoryServiceProxy.Current.Products.FirstOrDefault(invProd => invProd.Id == newProduct.Id);
            if (inventoryProduct == null)
            {
                return;
            }

            inventoryProduct.Quantity -= newProduct.Quantity;

            if (existingProduct != null)
            {
                // update
                existingProduct.Quantity += newProduct.Quantity;
            }
            else
            {
                //add
                cartToUse?.Contents?.Add(newProduct);
            }
        }

    }
}
