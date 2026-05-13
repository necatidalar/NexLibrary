using Microsoft.AspNetCore.Mvc;
using NexLibrary.Application.Interfaces.Services;

namespace NexLibrary.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public sealed class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken = default)
    {
        var result = await _dashboardService.GetSummaryAsync(cancellationToken);

        return result.BasariliMi ? Ok(result) : BadRequest(result);
    }
}