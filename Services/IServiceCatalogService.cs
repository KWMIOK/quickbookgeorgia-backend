using QuickBookGeorgia.API.DTOs;

namespace QuickBookGeorgia.API.Services;

public interface IServiceCatalogService
{
    Task<List<ServiceResponse>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default);
    Task<ServiceResponse> CreateAsync(CreateServiceRequest request, CancellationToken ct = default);
}
