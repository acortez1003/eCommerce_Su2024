using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Library.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<Product> Contents { get; set; } = new List<Product>();
        public string Name { get; set; }

        public ShoppingCart() { }

        public override string ToString()
        {
            return Name;
        }
    }
}
