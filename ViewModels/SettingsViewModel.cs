using CommunityToolkit.Mvvm.ComponentModel;

namespace LealTours.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty] private bool darkTheme;

    public void ApplyTheme()
    {
        Application.Current.UserAppTheme = DarkTheme ? AppTheme.Dark : AppTheme.Light;
    }
}