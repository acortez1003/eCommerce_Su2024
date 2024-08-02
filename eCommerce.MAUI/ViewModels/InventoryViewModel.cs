using System.ComponentModel;
using System.Runtime.CompilerServices;
using Amazon.Library.DTO;
using Amazon.Library.Models;
using Amazon.Library.Services;
using Amazon.Library.Services;

namespace eCommerce.MAUI.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public string? Query {  get; set; }
        public List<ProductViewModel> Products
        {
            get
            {
                return InventoryServiceProxy.Current.Products.Where(p => p != null)
                    .Select(p => new ProductViewModel(p)).ToList()
                    ?? new List<ProductViewModel>();
            }
        }

        public ProductViewModel? SelectedProduct { get; set; }

        public void Edit()
        {
            Shell.Current.GoToAsync($"//Product?productId={SelectedProduct?.Model?.Id ?? 0}");
        }

        public async Task Delete()
        {
            if(SelectedProduct != null)
            {
                await InventoryServiceProxy.Current.Delete(SelectedProduct.Model.Id);
                Refresh();
            }
        }

        public async void Refresh()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
