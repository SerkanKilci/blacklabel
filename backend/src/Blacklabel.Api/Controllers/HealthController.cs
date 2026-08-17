using Microsoft.AspNetCore.Mvc;

namespace Blacklabel.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(new { status = "ok", timestampUtc = DateTime.UtcNow });
}
