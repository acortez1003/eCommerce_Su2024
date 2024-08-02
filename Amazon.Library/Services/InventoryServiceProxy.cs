using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Library.DTO;

namespace Amazon.Library.Services
{
    public class InventoryServiceProxy
    {
        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();
        private List<ProductDTO> products;

        private InventoryServiceProxy()
        {
            products = new List<ProductDTO>
            {
                new ProductDTO {Id = 1, Name = "Apple", Description = "fruit", Price = 2.0m, Quantity = 50},
                new ProductDTO {Id = 2, Name = "Cucumber", Description = "veg", Price = 2.5m, Quantity = 25 }
            };
        }

        public ReadOnlyCollection<ProductDTO> Products
        {
            get
            {
                return products.AsReadOnly();
            }
        }

        public static InventoryServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new InventoryServiceProxy();
                    }
                }
                return instance;
            }
        }

        public Task<ProductDTO> AddOrUpdate(ProductDTO p)
        {
            var existingProduct = products.FirstOrDefault(prod => prod.Id == p.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = p.Name;
                existingProduct.Description = p.Description;
                existingProduct.Price = p.Price;
                existingProduct.Quantity = p.Quantity;
            }
            else
            {
                p.Id = products.Any() ? products.Max(prod => prod.Id) + 1 : 1;
                products.Add(p);
            }
            return Task.FromResult(p);
        }

        public Task<ProductDTO?> Delete(int id)
        {
            var itemToDelete = products.FirstOrDefault(p => p.Id == id);
            if (itemToDelete != null)
            {
                products.Remove(itemToDelete);
            }
            return Task.FromResult(itemToDelete);
        }
    }
}
