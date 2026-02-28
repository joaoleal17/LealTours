using LealTours.Models;
using Microsoft.EntityFrameworkCore;

namespace LealTours.Data;

public class AppDbContext : DbContext
{
    public string DbPath { get; }

    public AppDbContext()
    {
        // Local correto para guardar a BD em qualquer plataforma (Windows, Android, iOS)
        var folder = FileSystem.AppDataDirectory;
        DbPath = Path.Combine(folder, "lealtours.db");
    }

    // Tabelas
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}