using Amazon.Library.Services;
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

	private void TaxClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Tax");
	}

	private void MarkdownClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//Markdown");
	}

	private void BOGOClicked(object sender, EventArgs e)
	{
		var button = sender as Button;
		var viewModel = BindingContext as InventoryViewModel;

		if(viewModel != null && viewModel.SelectedProduct != null)
		{
			viewModel.SelectedProduct.IsBOGO = !viewModel.SelectedProduct.IsBOGO;

			var product = InventoryServiceProxy.Current.Products.
				FirstOrDefault(p => p.Id == viewModel.SelectedProduct?.Model?.Id);
			if(product != null)
			{
				product.IsBOGO = viewModel.SelectedProduct.IsBOGO;
				InventoryServiceProxy.Current.AddOrUpdate(product);
			}
			viewModel.Refresh();
		}
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		(BindingContext as InventoryViewModel)?.Refresh();
    }
}