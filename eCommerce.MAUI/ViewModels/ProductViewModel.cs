using Amazon.Library.Services;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Amazon.Library.Models;

namespace eCommerce.MAUI.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product? model;
        public Product? Model
        {
            get => model;
            set
            {
                model = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(DisplayPrice));
                NotifyPropertyChanged(nameof(DisplayMarkdown));
            }
        }

        public override string ToString()
        {
            if (Model == null)
            {
                return string.Empty;
            }
            return $"{Model.Id} - {Model.Name} - {Model.Price:C} - {Model.Markdown}";
        }

        private bool isBOGO;
        public bool IsBOGO
        {
            get => isBOGO;
            set
            {
                if(isBOGO != value)
                {
                    isBOGO = value;
                    NotifyPropertyChanged(nameof(IsBOGO));
                    NotifyPropertyChanged(nameof(DisplayPrice));
                }
            }
        }

        public string DisplayPrice
        {
            get
            {
                if(Model == null )
                {
                    return string.Empty;
                }
                return $"{Model.Price:C}";
            }
        }

        public string PriceAsString
        {
            set
            {
                if (Model == null)
                {
                    return;
                }
                if (decimal.TryParse(value, out var price))
                {
                    Model.Price = price;
                }
                else
                {

                }
            }
        }

        public string DisplayMarkdown
        {
            get
            {
                if (Model == null)
                {
                    return string.Empty;
                }
                return $"{Model.Markdown:C}";
            }
        }

        public string MarkdownAsString
        {
            set
            {
                if (Model == null)
                {
                    return;
                }
                if (decimal.TryParse(value, out var price))
                {
                    Model.Markdown = price;
                }
                else
                {

                }
            }
        }

        public decimal Markdown
        {
            get => Model?.Markdown ?? 0;
            set
            {
                if (Model != null)
                {
                    Model.Markdown = value;
                    NotifyPropertyChanged(nameof(Markdown));
                    NotifyPropertyChanged(nameof(DisplayMarkdown));
                }
            }
        }

        public ProductViewModel(int productId = 0)
        {
            if(productId == 0)
            {
                Model = new Product();
            }
            else
            {
                Model = InventoryServiceProxy
                    .Current
                    .Products.FirstOrDefault(p => p.Id == productId)
                    ?? new Product();
            }
        }

        public ProductViewModel(Product? model)
        {
            if(model != null)
            {
                Model = model;
                IsBOGO = model.IsBOGO;
            }
            else
            {
                Model = new Product();
            }
        }

        public void Add()
        {
            if(Model != null )
            {
                Model =  InventoryServiceProxy.Current.AddOrUpdate(Model);
                Refresh();
            }
        }

        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Model));
            NotifyPropertyChanged(nameof(Product));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
