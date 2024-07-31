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
            //TODO: stuff
        }

        public Task<ProductDTO?> Delete(int id)
        {
            var itemToDelete = Products.FirstOrDefault(p => p.Id == id);
            if(itemToDelete == null)
            {
                return null;
            }
            products.Remove(itemToDelete);
            return itemToDelete;
        }
    }
}
