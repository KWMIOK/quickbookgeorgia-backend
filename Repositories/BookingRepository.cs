using Microsoft.EntityFrameworkCore;
using QuickBookGeorgia.API.Data;
using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;

    public BookingRepository(AppDbContext db) => _db = db;

    public Task<List<Booking>> GetForBusinessOnDateAsync(Guid businessId, DateOnly date, CancellationToken ct = default) =>
        _db.Bookings
            .Include(b => b.Service)
            .Where(b => b.BusinessId == businessId && b.SelectedDate == date)
            .ToListAsync(ct);

    public Task<bool> ExistsAsync(Guid businessId, DateOnly date, TimeOnly time, CancellationToken ct = default) =>
        _db.Bookings.AnyAsync(b =>
            b.BusinessId == businessId &&
            b.SelectedDate == date &&
            b.SelectedTime == time, ct);

    public async Task AddAsync(Booking booking, CancellationToken ct = default) =>
        await _db.Bookings.AddAsync(booking, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
