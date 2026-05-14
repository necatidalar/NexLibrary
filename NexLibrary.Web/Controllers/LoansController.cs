using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Loans;

namespace NexLibrary.Web.Controllers;

public sealed class LoansController : Controller
{
    private readonly LoanApiService _loanApiService;
    private readonly BookApiService _bookApiService;
    private readonly MemberApiService _memberApiService;
    private readonly BookCopyApiService _bookCopyApiService;

    public LoansController(
        LoanApiService loanApiService,
        BookApiService bookApiService,
        MemberApiService memberApiService,
        BookCopyApiService bookCopyApiService)
    {
        _loanApiService = loanApiService;
        _bookApiService = bookApiService;
        _memberApiService = memberApiService;
        _bookCopyApiService = bookCopyApiService;
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

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var model = await CreateLoanCreateModelAsync(cancellationToken);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        LoanCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var filledModel = await CreateLoanCreateModelAsync(cancellationToken);

        filledModel.KitapId = model.KitapId;
        filledModel.UyeId = model.UyeId;
        filledModel.PlanlananIadeTarihi = model.PlanlananIadeTarihi;
        filledModel.Aciklama = model.Aciklama;

        if (model.KitapId <= 0)
        {
            TempData["ErrorMessage"] = "Kitap seçilmelidir.";
            return View(filledModel);
        }

        if (model.UyeId <= 0)
        {
            TempData["ErrorMessage"] = "Üye seçilmelidir.";
            return View(filledModel);
        }

        if (model.PlanlananIadeTarihi.Date < DateTime.Today)
        {
            TempData["ErrorMessage"] = "Planlanan iade tarihi bugünden küçük olamaz.";
            return View(filledModel);
        }

        var request = new LoanCreateRequest
        {
            KitapId = model.KitapId,
            KitapKopyaId = null,
            UyeId = model.UyeId,
            PlanlananIadeTarihi = model.PlanlananIadeTarihi.Date,
            Aciklama = string.IsNullOrWhiteSpace(model.Aciklama)
                ? null
                : model.Aciklama.Trim()
        };

        var result = await _loanApiService.CreateAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Ödünç verme işlemi başarısız. Kitabın müsait kopyası olmayabilir.";
            return View(filledModel);
        }

        TempData["SuccessMessage"] = "Kitap başarıyla ödünç verildi.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz ödünç kaydı.";
            return RedirectToAction(nameof(Index));
        }

        var request = new LoanReturnRequest
        {
            Aciklama = "Web üzerinden iade alındı."
        };

        var result = await _loanApiService.ReturnAsync(
            id,
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "İade alma işlemi başarısız. Kayıt zaten iade edilmiş veya iptal edilmiş olabilir.";
        }
        else
        {
            TempData["SuccessMessage"] = "Kitap başarıyla iade alındı.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<LoanCreateViewModel> CreateLoanCreateModelAsync(
        CancellationToken cancellationToken)
    {
        var bookResult = await _bookApiService.GetPagedAsync(
            1,
            1000,
            null,
            cancellationToken);

        var memberResult = await _memberApiService.GetPagedAsync(
            1,
            1000,
            null,
            cancellationToken);

        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync(
            cancellationToken);

        var availableBookIds = stockSummary
            .Where(x => x.Musait > 0)
            .Select(x => x.KitapId)
            .ToHashSet();

        var books = bookResult?.Items
            .Where(x => x.AktifMi && availableBookIds.Contains(x.Id))
            .OrderBy(x => x.KitapAdi)
            .ToList() ?? new List<NexLibrary.Contracts.Books.BookListResponse>();

        var members = memberResult?.Items
            .Where(x => x.AktifMi)
            .OrderBy(x => x.UyeAdiSoyadi)
            .ToList() ?? new List<NexLibrary.Contracts.Members.MemberListResponse>();

        return new LoanCreateViewModel
        {
            PlanlananIadeTarihi = DateTime.Today.AddDays(14),
            Books = books,
            Members = members
        };
    }
}