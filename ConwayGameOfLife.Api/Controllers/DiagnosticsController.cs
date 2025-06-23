using Microsoft.AspNetCore.Mvc;

namespace ConwayGameOfLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiagnosticsController : ControllerBase
{
    [Route("Heartbeat")]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Status = "OK", Timestamp = DateTime.UtcNow });
    }
}
