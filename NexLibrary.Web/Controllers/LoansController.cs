using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Loans;

namespace NexLibrary.Web.Controllers;

public sealed class LoansController : Controller
{
    private readonly LoanApiService _loanApiService;

    public LoansController(LoanApiService loanApiService)
    {
        _loanApiService = loanApiService;
    }

    public async Task<IActionResult> Index(
        string? search = null,
        bool overdueOnly = false,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 20 : pageSize;

        await _loanApiService.MarkOverdueAsync(cancellationToken);

        PagedResponse<LoanListResponse>? loans;

        if (overdueOnly)
        {
            loans = await _loanApiService.GetOverdueAsync(
                pageNumber,
                pageSize,
                cancellationToken);
        }
        else
        {
            loans = await _loanApiService.GetPagedAsync(
                pageNumber,
                pageSize,
                search,
                cancellationToken);
        }

        if (loans is null)
        {
            ViewBag.ErrorMessage = "API bağlantısı kurulamadı veya ödünç kayıtları alınamadı.";

            loans = new PagedResponse<LoanListResponse>
            {
                Items = new List<LoanListResponse>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
        }

        var model = new LoansIndexViewModel
        {
            Search = search,
            OverdueOnly = overdueOnly,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Loans = loans
        };

        return View(model);
    }
}