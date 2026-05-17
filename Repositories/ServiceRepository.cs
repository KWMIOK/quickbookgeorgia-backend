using Microsoft.EntityFrameworkCore;
using QuickBookGeorgia.API.Data;
using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly AppDbContext _db;

    public ServiceRepository(AppDbContext db) => _db = db;

    public Task<List<Service>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default) =>
        _db.Services
            .Where(s => s.BusinessId == businessId && s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(ct);

    public Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.Services.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddAsync(Service service, CancellationToken ct = default) =>
        await _db.Services.AddAsync(service, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
