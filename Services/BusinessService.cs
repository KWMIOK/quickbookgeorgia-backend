using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Entities;
using QuickBookGeorgia.API.Repositories;

namespace QuickBookGeorgia.API.Services;

public class BusinessService : IBusinessService
{
    private readonly IBusinessRepository _repo;

    public BusinessService(IBusinessRepository repo) => _repo = repo;

    public async Task<BusinessResponse?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var business = await _repo.GetBySlugWithServicesAsync(slug, ct);
        return business is null ? null : ToResponse(business);
    }

    public async Task<BusinessResponse> CreateAsync(CreateBusinessRequest request, CancellationToken ct = default)
    {
        var slug = !string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Slug!)
            : SlugHelper.Generate(request.Name);

        if (string.IsNullOrWhiteSpace(slug))
            throw new InvalidOperationException("Could not generate a valid slug from name.");

        var unique = slug;
        var suffix = 2;
        while (await _repo.SlugExistsAsync(unique, ct))
        {
            unique = $"{slug}-{suffix++}";
        }

        var workingHours = request.WorkingHours ?? WorkingHoursHelper.DefaultWeek();

        var business = new Business
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Slug = unique,
            PhoneNumber = request.PhoneNumber.Trim(),
            Description = request.Description?.Trim(),
            WorkingHoursJson = WorkingHoursHelper.Serialize(workingHours),
            SlotIntervalMinutes = request.SlotIntervalMinutes <= 0 ? 30 : request.SlotIntervalMinutes,
            CreatedAt = DateTime.UtcNow,
        };

        await _repo.AddAsync(business, ct);
        await _repo.SaveChangesAsync(ct);

        return ToResponse(business);
    }

    private static BusinessResponse ToResponse(Business b) => new()
    {
        Id = b.Id,
        Name = b.Name,
        Slug = b.Slug,
        PhoneNumber = b.PhoneNumber,
        Description = b.Description,
        WorkingHours = WorkingHoursHelper.Deserialize(b.WorkingHoursJson),
        SlotIntervalMinutes = b.SlotIntervalMinutes,
        CreatedAt = b.CreatedAt,
        Services = b.Services.Where(s => s.IsActive).Select(s => new ServiceResponse
        {
            Id = s.Id,
            BusinessId = s.BusinessId,
            Name = s.Name,
            DurationMinutes = s.DurationMinutes,
            Price = s.Price,
            IsActive = s.IsActive,
        }).ToList(),
    };
}
