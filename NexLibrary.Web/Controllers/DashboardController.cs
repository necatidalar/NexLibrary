using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Dashboard;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;

namespace NexLibrary.Web.Controllers;

public sealed class DashboardController : Controller
{
    private readonly DashboardApiService _dashboardApiService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        DashboardApiService dashboardApiService,
        ILogger<DashboardController> logger)
    {
        _dashboardApiService = dashboardApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.DashboardView)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        DashboardSummaryResponse? summary = null;

        try
        {
            summary = await _dashboardApiService.GetSummaryAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard özeti alınırken hata oluştu.");
        }

        if (summary is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya dashboard verisi alınamadı.";
            summary = new DashboardSummaryResponse();
        }

        return View(summary);
    }
}
