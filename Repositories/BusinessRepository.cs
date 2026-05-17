using Microsoft.EntityFrameworkCore;
using QuickBookGeorgia.API.Data;
using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public class BusinessRepository : IBusinessRepository
{
    private readonly AppDbContext _db;

    public BusinessRepository(AppDbContext db) => _db = db;

    public Task<Business?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.Businesses.FirstOrDefaultAsync(b => b.Id == id, ct);

    public Task<Business?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        _db.Businesses.FirstOrDefaultAsync(b => b.Slug == slug, ct);

    public Task<Business?> GetBySlugWithServicesAsync(string slug, CancellationToken ct = default) =>
        _db.Businesses
            .Include(b => b.Services.Where(s => s.IsActive))
            .FirstOrDefaultAsync(b => b.Slug == slug, ct);

    public Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default) =>
        _db.Businesses.AnyAsync(b => b.Slug == slug, ct);

    public async Task AddAsync(Business business, CancellationToken ct = default) =>
        await _db.Businesses.AddAsync(business, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
