using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.Entities;

public class Business
{
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Slug { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Stored as JSON. Example:
    /// { "monday": { "open": "09:00", "close": "18:00" }, "tuesday": null, ... }
    /// A null entry (or missing day) means the business is closed that day.
    /// </summary>
    public string WorkingHoursJson { get; set; } = "{}";

    /// <summary>
    /// Slot granularity in minutes used when generating bookable time slots.
    /// </summary>
    public int SlotIntervalMinutes { get; set; } = 30;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Service> Services { get; set; } = new List<Service>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
