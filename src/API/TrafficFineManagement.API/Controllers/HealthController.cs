using Microsoft.AspNetCore.Mvc;

namespace TrafficFineManagament.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new HealthResponse("OK", DateTimeOffset.UtcNow));
    }
}

public sealed record HealthResponse(string Status, DateTimeOffset CheckedAt);
