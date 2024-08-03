using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Amazon.Library.Models;
using Amazon.Library.Services;

namespace eCommerce.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public ShopViewModel()
        {
            InventoryQuery = string.Empty;
            SelectedCart = Carts.FirstOrDefault();
        }

        private string inventoryQuery;
        public string InventoryQuery
        {
            set
            {
                inventoryQuery = value;
                NotifyPropertyChanged(nameof(Products));
            }
            get
            {
                return inventoryQuery;
            }
        }

        public List<ProductViewModel> Products
        {
            get
            {
                return InventoryServiceProxy.Current.Products
                    .Where(p => p != null)
                    .Where(p => p.Quantity > 0)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }

        private ShoppingCart? selectedCart;
        public ShoppingCart? SelectedCart
        {
            get
            {
                return selectedCart;
            }
            set
            {
                selectedCart = value;
                NotifyPropertyChanged(nameof(SelectedCart));
                NotifyPropertyChanged(nameof(ProductsInCart));
                NotifyPropertyChanged(nameof(TotalPrice));
                NotifyPropertyChanged(nameof(TaxRate));
                NotifyPropertyChanged(nameof(BOGODiscount));
                NotifyPropertyChanged(nameof(TotalPriceWithTax));
            }
        }

        public ObservableCollection<ShoppingCart> Carts
        {
            get
            {
                return new ObservableCollection<ShoppingCart>(ShoppingCartServiceProxy.Current.Carts);
            }
        }

        public List<ProductViewModel> ProductsInCart
        {
            get
            {
                return SelectedCart?.Contents?.Where(p => p != null)
                    .Where(p => p?.Name?.ToUpper()?.Contains(InventoryQuery.ToUpper()) ?? false)
                    .Select(p => new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }

        private ProductViewModel? productToBuy;
        public ProductViewModel? ProductToBuy
        {
            get => productToBuy;
            set
            {
                productToBuy = value;
                if (productToBuy != null && productToBuy.Model == null)
                {
                    productToBuy.Model = new Product();
                } else if (productToBuy != null && productToBuy != null)
                {
                    productToBuy.Model = new Product(productToBuy.Model);
                }
            }
        }

        public decimal TaxRate
        {
            get => ShoppingCartServiceProxy.Current.TaxRate;
            set
            {
                if (ShoppingCartServiceProxy.Current.TaxRate != value)
                {
                    ShoppingCartServiceProxy.Current.TaxRate = value;
                    NotifyPropertyChanged(nameof(TaxRate));
                    NotifyPropertyChanged(nameof(TotalPriceWithTax));
                }
            }
        }

        public decimal TotalPrice
        {
            get
            {
                if (SelectedCart == null || SelectedCart.Contents == null)
                    return 0;

                return SelectedCart.Contents.Sum(p =>                
                {
                    var price = p.Price;
                    var markdown = p.Markdown;

                    if (markdown != 0)
                    {
                        return (price - markdown) * p.Quantity;
                    }
                    return price * p.Quantity;
                });
            }
        }

        public decimal BOGODiscount
        {
            get
            {
                if (SelectedCart == null || SelectedCart.Contents == null)
                    return 0;
                return SelectedCart.Contents.Sum(p =>
                {
                    var product = p as Product;
                    if (product != null && InventoryServiceProxy.Current.IsBOGO(product.Id))
                    {
                        int fq = product.Quantity / 2;
                        return product.Price * fq;
                    }
                    return 0;
                });
            }
        }

        public decimal TotalWithDiscount
        {
            get
            {
                return TotalPrice - BOGODiscount;
            }
        }

        public decimal TotalPriceWithTax
        {
            get
            {
                decimal totalPrice = TotalWithDiscount;
                return totalPrice + (totalPrice * (TaxRate/100));
            }
        }

        public void Refresh()
        {
            InventoryQuery = string.Empty;
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Carts));
        }

        public void PlaceInCart()
        {
            if (ProductToBuy?.Model == null)
            {
                return;
            }
            ProductToBuy.Model.Quantity = 1;
            ShoppingCartServiceProxy.Current.AddToCart(ProductToBuy.Model, SelectedCart.Id);

            ProductToBuy = null;
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Carts));
            NotifyPropertyChanged(nameof(ProductsInCart));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
