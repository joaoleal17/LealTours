using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LealTours.Data;
using LealTours.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace LealTours.ViewModels;

public partial class ManageToursViewModel : BaseViewModel
{
    public ObservableCollection<Tour> Items { get; } = new();

    [ObservableProperty] private Tour? selected;
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string? description;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private int durationHours;
    [ObservableProperty] private bool isActive = true;

    public async Task LoadAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            using var db = new AppDbContext();
            var list = await db.Tours.OrderBy(t => t.Name).ToListAsync();
            Items.Clear();
            foreach (var t in list) Items.Add(t);
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public void Edit(Tour? tour)
    {
        if (tour is null)
        {
            Selected = null;
            Name = string.Empty;
            Description = null;
            Price = 0;
            DurationHours = 0;
            IsActive = true;
            return;
        }

        Selected = tour;
        Name = tour.Name;
        Description = tour.Description;
        Price = tour.Price;
        DurationHours = tour.DurationHours;
        IsActive = tour.IsActive;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException("Nome é obrigatório.");
        if (DurationHours <= 0) throw new InvalidOperationException("Duração inválida.");

        using var db = new AppDbContext();

        if (Selected is null)
        {
            var entity = new Tour
            {
                Name = Name.Trim(),
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description!.Trim(),
                Price = Price,
                DurationHours = DurationHours,
                IsActive = IsActive
            };
            db.Tours.Add(entity);
            await db.SaveChangesAsync();
            Items.Add(entity);
        }
        else
        {
            Selected.Name = Name.Trim();
            Selected.Description = string.IsNullOrWhiteSpace(Description) ? null : Description!.Trim();
            Selected.Price = Price;
            Selected.DurationHours = DurationHours;
            Selected.IsActive = IsActive;

            db.Tours.Update(Selected);
            await db.SaveChangesAsync();

            var idx = Items.IndexOf(Items.First(x => x.Id == Selected.Id));
            Items[idx] = Selected; // refrescar binding
        }

        Edit(null);
    }

    [RelayCommand]
    public async Task DeleteAsync(Tour tour)
    {
        using var db = new AppDbContext();
        var entity = await db.Tours.FindAsync(tour.Id);
        if (entity is null) return;
        db.Tours.Remove(entity);
        await db.SaveChangesAsync();
        Items.Remove(tour);
    }
}