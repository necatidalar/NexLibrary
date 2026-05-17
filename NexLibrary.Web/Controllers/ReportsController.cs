using Microsoft.AspNetCore.Mvc;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Reports;

namespace NexLibrary.Web.Controllers;

public sealed class ReportsController : Controller
{
    private readonly BookApiService _bookApiService;
    private readonly MemberApiService _memberApiService;
    private readonly LoanApiService _loanApiService;
    private readonly BookCopyApiService _bookCopyApiService;

    public ReportsController(
        BookApiService bookApiService,
        MemberApiService memberApiService,
        LoanApiService loanApiService,
        BookCopyApiService bookCopyApiService)
    {
        _bookApiService = bookApiService;
        _memberApiService = memberApiService;
        _loanApiService = loanApiService;
        _bookCopyApiService = bookCopyApiService;
    }

    public async Task<IActionResult> Index(
        string? search = null,
        string? loanStatus = "Tumu",
        CancellationToken cancellationToken = default)
    {
        await _loanApiService.MarkOverdueAsync(cancellationToken);

        var booksResult = await _bookApiService.GetPagedAsync(
            1,
            1000,
            search,
            cancellationToken);

        var membersResult = await _memberApiService.GetPagedAsync(
            1,
            1000,
            search,
            cancellationToken);

        var loansResult = await _loanApiService.GetPagedAsync(
            1,
            1000,
            search,
            cancellationToken);

        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync(
            cancellationToken);

        var loans = loansResult?.Items.ToList()
            ?? new List<NexLibrary.Contracts.Loans.LoanListResponse>();

        if (!string.IsNullOrWhiteSpace(loanStatus) && loanStatus != "Tumu")
        {
            loans = loans
                .Where(x => x.Durum == loanStatus)
                .ToList();
        }

        var model = new ReportsIndexViewModel
        {
            Search = search,
            LoanStatus = string.IsNullOrWhiteSpace(loanStatus) ? "Tumu" : loanStatus,
            Books = booksResult?.Items.ToList()
                ?? new List<NexLibrary.Contracts.Books.BookListResponse>(),
            Members = membersResult?.Items.ToList()
                ?? new List<NexLibrary.Contracts.Members.MemberListResponse>(),
            Loans = loans,
            StockSummary = stockSummary
        };

        return View(model);
    }
}