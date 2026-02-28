using LealTours.Models;
using LealTours.Views; // <-- garante que tens este using

namespace LealTours.Views;

public partial class ToursPage : ContentPage
{
    public ToursPage()
    {
        InitializeComponent();

        Loaded += async (_, __) =>
        {
            if (BindingContext is ViewModels.ToursViewModel vm)
                await vm.LoadAsync();
        };
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection?.FirstOrDefault() is LealTours.Models.Tour selected)
        {
            await Shell.Current.GoToAsync(nameof(LealTours.Views.BookingPage), new Dictionary<string, object>
            {
                ["tour"] = selected
            });

            (sender as CollectionView)!.SelectedItem = null;
        }
    }
}