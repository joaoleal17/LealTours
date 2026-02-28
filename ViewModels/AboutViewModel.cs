using CommunityToolkit.Mvvm.ComponentModel;

namespace LealTours.ViewModels;

public partial class AboutViewModel : BaseViewModel
{
    public string AppName => "LealTours";
    public string Version => "1.0.0";
    public string Author => "João Leal";
}