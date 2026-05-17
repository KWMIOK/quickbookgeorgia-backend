using System.ComponentModel.DataAnnotations;

namespace QuickBookGeorgia.API.DTOs;

public class CreateBookingRequest
{
    [Required] public Guid BusinessId { get; set; }
    [Required] public Guid ServiceId { get; set; }

    [Required, MaxLength(120)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string CustomerPhone { get; set; } = string.Empty;

    /// <summary>YYYY-MM-DD</summary>
    [Required] public string SelectedDate { get; set; } = string.Empty;

    /// <summary>HH:mm</summary>
    [Required] public string SelectedTime { get; set; } = string.Empty;
}

public class BookingResponse
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string SelectedDate { get; set; } = string.Empty;
    public string SelectedTime { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    /// <summary>The wa.me link to send the owner with the booking details.</summary>
    public string WhatsAppUrl { get; set; } = string.Empty;
}

public class AvailableSlotsResponse
{
    public string Date { get; set; } = string.Empty;
    public List<string> Slots { get; set; } = new();
}
