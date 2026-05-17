using Microsoft.AspNetCore.Mvc;
using QuickBookGeorgia.API.DTOs;
using QuickBookGeorgia.API.Services;

namespace QuickBookGeorgia.API.Controllers;

[ApiController]
[Route("api/business")]
public class BusinessController : ControllerBase
{
    private readonly IBusinessService _service;

    public BusinessController(IBusinessService service) => _service = service;

    [HttpGet("{slug}")]
    public async Task<ActionResult<BusinessResponse>> GetBySlug(string slug, CancellationToken ct)
    {
        var business = await _service.GetBySlugAsync(slug, ct);
        return business is null ? NotFound(new { error = "Business not found." }) : Ok(business);
    }

    [HttpPost]
    public async Task<ActionResult<BusinessResponse>> Create([FromBody] CreateBusinessRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetBySlug), new { slug = created.Slug }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
