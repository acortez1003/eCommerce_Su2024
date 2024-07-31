namespace eCommerce.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ShopClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Shop");
        }

        private void InventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Inventory");
        }
    }
}
