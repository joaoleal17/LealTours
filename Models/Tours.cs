using System.ComponentModel.DataAnnotations;

namespace LealTours.Models;

public class Tour
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = default!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(1, 240)]
    public int DurationHours { get; set; }

    public bool IsActive { get; set; } = true;
}