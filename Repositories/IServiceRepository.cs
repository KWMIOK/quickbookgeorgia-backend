using QuickBookGeorgia.API.Entities;

namespace QuickBookGeorgia.API.Repositories;

public interface IServiceRepository
{
    Task<List<Service>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Service service, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
