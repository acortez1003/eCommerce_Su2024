using System.Collections.ObjectModel;
using Amazon.Library.Models;

namespace Amazon.Library.Services
{
    public class InventoryServiceProxy
    {
        private static InventoryServiceProxy? instance;
        private static object instanceLock = new object();

        private List<Product> products;

        public ReadOnlyCollection<Product> Products
        {
            get
            {
                return products.AsReadOnly();
            }
        }

        private int NextId
        {
            get
            {
                if (!products.Any())
                {
                    return 1;
                }

                return products.Select(p => p.Id).Max() + 1;
            }
        }

        public Product AddOrUpdate(Product p)
        {
            if (p.Id == 0)
            {
                p.Id = NextId;
                products.Add(p);
            }
            else
            {
                var existingProduct = products.FirstOrDefault(prod => prod.Id == p.Id);
                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.Name = p.Name;
                    existingProduct.Description = p.Description;
                    existingProduct.Price = p.Price;
                    existingProduct.Markdown = p.Markdown; // Update the markdown value
                    existingProduct.Quantity = p.Quantity;
                    existingProduct.IsBOGO = p.IsBOGO;
                }
            }

            return p;
        }

        public bool Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if(product != null)
            {
                products.Remove(product);
                return true;
            }
            return false;
        }

        public bool IsBOGO(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            return product?.IsBOGO ?? false;
        }

        private InventoryServiceProxy()
        {
            products = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Description = "Fruit", Price = 2.0m, Quantity = 25 },
                new Product { Id = 2, Name = "Cucumber", Description = "Veggie", Price = 1.5m, Quantity = 50 },
                new Product { Id = 3, Name = "Milk", Description = "White", Price = 10m, Quantity = 34},

                new Product { Id = 4, Name = "Textbook", Description = "Expensive", Price = 200m, Quantity = 200},
                new Product { Id = 5, Name = "Notebook", Description = "Has papers", Price = 7.0m, Quantity = 75},
                new Product { Id = 6, Name = "Agenda" , Description = "Counts days", Price = 15m, Quantity = 35}
            };
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
    }
}
