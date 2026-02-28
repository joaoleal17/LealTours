namespace LealTours.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private void OnToggled(object sender, ToggledEventArgs e)
    {
        if (BindingContext is ViewModels.SettingsViewModel vm)
            vm.ApplyTheme();
    }
}