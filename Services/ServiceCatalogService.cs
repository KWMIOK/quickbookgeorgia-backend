using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Entities;
using QuickBookGeorgia.API.Repositories;

namespace QuickBookGeorgia.API.Services;

public class ServiceCatalogService : IServiceCatalogService
{
    private readonly IServiceRepository _services;
    private readonly IBusinessRepository _businesses;

    public ServiceCatalogService(IServiceRepository services, IBusinessRepository businesses)
    {
        _services = services;
        _businesses = businesses;
    }

    public async Task<List<ServiceResponse>> GetByBusinessIdAsync(Guid businessId, CancellationToken ct = default)
    {
        var services = await _services.GetByBusinessIdAsync(businessId, ct);
        return services.Select(ToResponse).ToList();
    }

    public async Task<ServiceResponse> CreateAsync(CreateServiceRequest request, CancellationToken ct = default)
    {
        var business = await _businesses.GetByIdAsync(request.BusinessId, ct)
            ?? throw new KeyNotFoundException($"Business {request.BusinessId} not found.");

        var service = new Service
        {
            Id = Guid.NewGuid(),
            BusinessId = business.Id,
            Name = request.Name.Trim(),
            DurationMinutes = request.DurationMinutes,
            Price = request.Price,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
        };

        await _services.AddAsync(service, ct);
        await _services.SaveChangesAsync(ct);

        return ToResponse(service);
    }

    private static ServiceResponse ToResponse(Service s) => new()
    {
        Id = s.Id,
        BusinessId = s.BusinessId,
        Name = s.Name,
        DurationMinutes = s.DurationMinutes,
        Price = s.Price,
        IsActive = s.IsActive,
    };
}
