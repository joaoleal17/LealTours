using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LealTours.Data;
using LealTours.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace LealTours.ViewModels;

public partial class ManageBookingsViewModel : BaseViewModel
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
            foreach (var b in list) Items.Add(b);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task CancelAsync(Booking booking)
    {
        using var db = new AppDbContext();
        var entity = await db.Bookings.FindAsync(booking.Id);
        if (entity is null) return;
        db.Bookings.Remove(entity);
        await db.SaveChangesAsync();
        Items.Remove(booking);
    }
}