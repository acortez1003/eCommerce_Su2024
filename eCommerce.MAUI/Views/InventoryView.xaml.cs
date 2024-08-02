using eCommerce.MAUI.ViewModels;
namespace eCommerce.MAUI.Views;

public partial class InventoryView : ContentPage
{
	public InventoryView()
	{
		InitializeComponent();
		BindingContext = new InventoryViewModel();
	}

	private async void CancelClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}

	private void AddClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Product");
	}

	private void EditClicked(object sender, EventArgs e)
	{
		(BindingContext as InventoryViewModel)?.Edit();
	}

	private void DeleteClicked(object sender, EventArgs e)
	{
		(BindingContext as InventoryViewModel)?.Delete();
	}
}