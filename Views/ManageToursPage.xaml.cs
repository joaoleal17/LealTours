namespace LealTours.Views;

public partial class ManageToursPage : ContentPage
{
    public ManageToursPage()
    {
        InitializeComponent();
        Loaded += async (_, __) =>
        {
            if (BindingContext is ViewModels.ManageToursViewModel vm)
                await vm.LoadAsync();
        };
    }
}