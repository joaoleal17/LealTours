using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LealTours.Data;
using LealTours.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace LealTours.ViewModels;

public partial class ToursViewModel : BaseViewModel
{
    public ObservableCollection<Tour> Items { get; } = new();

    [ObservableProperty]
    Tour? selected;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            using var db = new AppDbContext();
            var tours = await db.Tours
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();

            Items.Clear();
            foreach (var t in tours) Items.Add(t);
        }
        finally
        {
            IsBusy = false;
        }
    }
}