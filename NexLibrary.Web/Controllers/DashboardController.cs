using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Dashboard;
using NexLibrary.Web.Services;

namespace NexLibrary.Web.Controllers;

public sealed class DashboardController : Controller
{
    private readonly DashboardApiService _dashboardApiService;

    public DashboardController(DashboardApiService dashboardApiService)
    {
        _dashboardApiService = dashboardApiService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var summary = await _dashboardApiService.GetSummaryAsync(cancellationToken);

        if (summary is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya dashboard verisi alınamadı.";
            summary = new DashboardSummaryResponse();
        }

        return View(summary);
    }
}