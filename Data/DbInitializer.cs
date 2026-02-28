using System.Diagnostics;
using LealTours.Models;
using Microsoft.EntityFrameworkCore;

namespace LealTours.Data;

public static class DbInitializer
{
    public static async Task EnsureCreatedAndSeedAsync()
    {
        using var db = new AppDbContext();

        Debug.WriteLine($"[DB PATH] {db.DbPath}");

        // Cria a BD e o schema se ainda não existir
        await db.Database.EnsureCreatedAsync();
        Debug.WriteLine("[DB CHECK] EnsureCreatedAsync() ok");

        // Seed inicial
        if (!await db.Tours.AnyAsync())
        {
            db.Tours.AddRange(
                new Tour { Name = "Sintra Palácios", Description = "Pena + Regaleira", Price = 59.9m, DurationHours = 6, IsActive = true },
                new Tour { Name = "Lisboa Essencial", Description = "Alfama + Belém", Price = 39.9m, DurationHours = 4, IsActive = true }
            );
            await db.SaveChangesAsync();
            Debug.WriteLine("[DB SEED] Inseridos tours iniciais");
        }

        var count = await db.Tours.CountAsync();
        Debug.WriteLine($"[DB CHECK] Tours count = {count}");
    }
}