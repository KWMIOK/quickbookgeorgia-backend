using Microsoft.AspNetCore.Mvc;
using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Services;

namespace QuickBookGeorgia.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly IServiceCatalogService _catalog;

    public ServicesController(IServiceCatalogService catalog) => _catalog = catalog;

    [HttpGet("{businessId:guid}")]
    public async Task<ActionResult<List<ServiceResponse>>> GetByBusiness(Guid businessId, CancellationToken ct)
    {
        var services = await _catalog.GetByBusinessIdAsync(businessId, ct);
        return Ok(services);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse>> Create([FromBody] CreateServiceRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var created = await _catalog.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetByBusiness), new { businessId = created.BusinessId }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
