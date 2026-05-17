using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public interface IBookingRepository
{
    Task<List<Booking>> GetForBusinessOnDateAsync(Guid businessId, DateOnly date, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid businessId, DateOnly date, TimeOnly time, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
