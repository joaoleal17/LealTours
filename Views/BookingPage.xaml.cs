using LealTours.Models;
using LealTours.ViewModels;

namespace LealTours.Views;

[QueryProperty(nameof(Tour), "tour")]
public partial class BookingPage : ContentPage
{
    public BookingPage()
    {
        InitializeComponent();
    }

    // Propriedade que vai receber o Tour via Shell
    public Tour Tour
    {
        get => (BindingContext as BookingViewModel)!.Tour;
        set
        {
            if (BindingContext is BookingViewModel vm && value != null)
                vm.Initialize(value);
        }
    }
}