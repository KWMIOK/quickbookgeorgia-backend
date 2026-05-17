using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public interface IBusinessRepository
{
    Task<Business?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Business?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Business?> GetBySlugWithServicesAsync(string slug, CancellationToken ct = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);
    Task AddAsync(Business business, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
