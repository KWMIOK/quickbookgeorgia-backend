using Microsoft.AspNetCore.Mvc;
using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Services;

namespace QuickBookGeorgia.API.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookings;

    public BookingsController(IBookingService bookings) => _bookings = bookings;

    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Create([FromBody] CreateBookingRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var booking = await _bookings.CreateAsync(request, ct);
            return Ok(booking);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    /// <summary>
    /// GET /api/bookings/slots?businessId=...&serviceId=...&date=yyyy-MM-dd
    /// </summary>
    [HttpGet("slots")]
    public async Task<ActionResult<AvailableSlotsResponse>> GetSlots(
        [FromQuery] Guid businessId,
        [FromQuery] Guid serviceId,
        [FromQuery] string date,
        CancellationToken ct)
    {
        try
        {
            var slots = await _bookings.GetAvailableSlotsAsync(businessId, serviceId, date, ct);
            return Ok(slots);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
