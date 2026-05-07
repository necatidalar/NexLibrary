using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var response = ApiResponse<object>.Success(new
        {
            Status = "Healthy",
            Application = "NexLibrary.Api",
            Date = DateTime.UtcNow
        });

        return Ok(response);
    }
}