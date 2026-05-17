using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.DTOs;

public class CreateServiceRequest
{
    [Required]
    public Guid BusinessId { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Range(5, 480)]
    public int DurationMinutes { get; set; } = 30;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}

public class ServiceResponse
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
