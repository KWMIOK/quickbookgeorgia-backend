using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.Entities;

public class Booking
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }

    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }

    [Required, MaxLength(120)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string CustomerPhone { get; set; } = string.Empty;

    public DateOnly SelectedDate { get; set; }

    public TimeOnly SelectedTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
