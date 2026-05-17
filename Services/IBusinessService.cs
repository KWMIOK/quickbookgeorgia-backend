using QuickBookGeorgia.API.DTOs;

namespace QuickBookGeorgia.API.Services;

public interface IBusinessService
{
    Task<BusinessResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<BusinessResponse> CreateAsync(CreateBusinessRequest request, CancellationToken ct = default);
}
