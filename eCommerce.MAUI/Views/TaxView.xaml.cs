using eCommerce.MAUI.ViewModels;

namespace eCommerce.MAUI.Views;

public partial class TaxView : ContentPage
{
	public TaxView()
	{
		InitializeComponent();
        BindingContext = new ShopViewModel();
	}

    private async void OkClicked(object sender, EventArgs e)
    {
        if (decimal.TryParse(TaxEntry.Text, out decimal tax) && tax >= 1 && tax <= 15)
        {
            (BindingContext as ShopViewModel).TaxRate = tax;
            await Shell.Current.GoToAsync("//Inventory");
        }
        else
        {
            ErrorLabel.Text = "Please enter a valid tax percentage between 1 and 15.";
        }
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Inventory");
    }
}