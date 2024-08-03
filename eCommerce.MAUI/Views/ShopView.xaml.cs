using eCommerce.MAUI.ViewModels;

namespace eCommerce.MAUI.Views;

public partial class ShopView : ContentPage
{
    public ShopView()
    {
        InitializeComponent();
        BindingContext = new ShopViewModel();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel).Refresh();
    }

    private void PlaceInCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel).PlaceInCart();
    }

    private void AddCartClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Cart");
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ShopViewModel)?.Refresh();
    }
}