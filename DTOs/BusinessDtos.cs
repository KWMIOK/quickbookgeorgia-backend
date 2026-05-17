using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.DTOs;

public class CreateBusinessRequest
{
    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional. If omitted, a slug is generated from Name.</summary>
    [MaxLength(60)]
    public string? Slug { get; set; }

    [Required, MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Object keyed by day-of-week (lowercase). Each value: { "open": "HH:mm", "close": "HH:mm" } or null.
    /// </summary>
    public Dictionary<string, DayHours?>? WorkingHours { get; set; }

    public int SlotIntervalMinutes { get; set; } = 30;
}

public class DayHours
{
    [Required] public string Open { get; set; } = "09:00";
    [Required] public string Close { get; set; } = "18:00";
}

public class BusinessResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Dictionary<string, DayHours?> WorkingHours { get; set; } = new();
    public int SlotIntervalMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ServiceResponse> Services { get; set; } = new();
}
