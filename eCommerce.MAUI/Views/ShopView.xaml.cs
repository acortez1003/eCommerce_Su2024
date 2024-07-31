using eCommerce.MAUI.ViewModels;

namespace eCommerce.MAUI.Views;

public partial class ShopView : ContentPage
{
	public ShopView()
	{
		InitializeComponent();
		BindingContext = new ShopViewModel();
	}
    private void BackClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}