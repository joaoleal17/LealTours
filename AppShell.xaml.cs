using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace LealTours;

public partial class App : Application
{
    private readonly AppShell _shell;

    // AppShell é injetado via DI
    public App(AppShell shell)
    {
        InitializeComponent();
        _shell = shell;
        // ❌ Antigo (obsoleto em .NET 9 no Windows):
        // MainPage = new AppShell();
    }

    /// <summary>
    /// .NET 9+: define a janela raiz da aplicação aqui (evita CS0618).
    /// </summary>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_shell);
    }
}