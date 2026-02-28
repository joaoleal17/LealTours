namespace LealTours;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // (Opcional) registar rotas de navegação por Shell:
        Routing.RegisterRoute(nameof(Views.BookingPage),
            typeof(Views.BookingPage));
        // Routing.RegisterRoute(nameof(Views.BookingPage), typeof(Views.BookingPage));
    }
}