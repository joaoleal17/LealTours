namespace LealTours;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // A página principal da app é o Shell que criámos acima
        MainPage = new NavigationPage(new AppShell());

        // (Se usas seed da BD, mantém a chamada que tinhas)
        Task.Run(Data.DbInitializer.EnsureCreatedAndSeedAsync);
    }
}