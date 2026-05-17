using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.Entities;

public class Service
{
    public Guid Id { get; set; }

    public Guid BusinessId { get; set; }
    public Business? Business { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    public int DurationMinutes { get; set; } = 30;

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
