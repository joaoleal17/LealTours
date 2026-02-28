using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace LealTours;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ⬇️ Regista o Shell (padrão recomendado)
        builder.Services.AddSingleton<AppShell>();

        // ⬇️ (Exemplos) registos de páginas/viewmodels se usas DI
        // builder.Services.AddTransient<MainPage>();
        // builder.Services.AddTransient<BookingPage>();
        // builder.Services.AddTransient<ManageBookingPage>();

        return builder.Build();
    }
}