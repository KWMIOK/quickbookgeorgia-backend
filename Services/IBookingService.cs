using QuickBookGeorgia.API.DTOs;

namespace QuickBookGeorgia.API.Services;

public interface IBookingService
{
    Task<BookingResponse> CreateAsync(CreateBookingRequest request, CancellationToken ct = default);
    Task<AvailableSlotsResponse> GetAvailableSlotsAsync(Guid businessId, Guid serviceId, string date, CancellationToken ct = default);
}
