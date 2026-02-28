using CommunityToolkit.Mvvm.ComponentModel;
using LealTours.Data;
using LealTours.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace LealTours.ViewModels;

public partial class BookingViewModel : BaseViewModel
{
    public ObservableCollection<Booking> Items { get; } = new();

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;

            using var db = new AppDbContext();
            var list = await db.Bookings
                .Include(b => b.Tour)
                .OrderByDescending(b => b.Date)
                .ThenByDescending(b => b.Id)
                .ToListAsync();

            Items.Clear();
            foreach (var b in list)
                Items.Add(b);
        }
        finally
        {
            IsBusy = false;
        }
    }
}