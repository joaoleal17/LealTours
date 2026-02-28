using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LealTours.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public int TourId { get; set; }

    [ForeignKey(nameof(TourId))]
    public Tour Tour { get; set; } = default!;

    [Required, StringLength(120)]
    public string CustomerName { get; set; } = default!;

    [Required, EmailAddress]
    public string CustomerEmail { get; set; } = default!;

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Range(1, 1000)]
    public int People { get; set; } = 1;
}