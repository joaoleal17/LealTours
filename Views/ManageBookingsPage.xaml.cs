namespace LealTours.Views;

public partial class ManageBookingPage : ContentPage
{
    public ManageBookingPage()
    {
        InitializeComponent();
        Loaded += async (_, __) =>
        {
            if (BindingContext is ViewModels.ManageBookingsViewModel vm)
                await vm.LoadAsync();
        };
    }
}