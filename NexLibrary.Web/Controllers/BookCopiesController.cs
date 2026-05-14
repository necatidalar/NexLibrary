using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.BookCopies;

namespace NexLibrary.Web.Controllers;

public sealed class BookCopiesController : Controller
{
    private readonly BookCopyApiService _bookCopyApiService;

    public BookCopiesController(BookCopyApiService bookCopyApiService)
    {
        _bookCopyApiService = bookCopyApiService;
    }

    public async Task<IActionResult> Index(
        int? kitapId = null,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync(cancellationToken);

        var model = new BookCopiesIndexViewModel
        {
            SelectedBookId = kitapId,
            StockSummary = stockSummary
        };

        if (kitapId.HasValue && kitapId.Value > 0)
        {
            var selectedBook = stockSummary.FirstOrDefault(x => x.KitapId == kitapId.Value);

            model.SelectedBookName = selectedBook?.KitapAdi;
            model.Copies = await _bookCopyApiService.GetByBookIdAsync(
                kitapId.Value,
                cancellationToken);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create(
        int? kitapId = null,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync(cancellationToken);

        var model = new BookCopyCreateViewModel
        {
            KitapId = kitapId ?? 0,
            Books = stockSummary
                .OrderBy(x => x.KitapAdi)
                .ToList()
        };

        if (kitapId.HasValue && kitapId.Value > 0)
        {
            model.Barkod = GenerateBarcode(kitapId.Value);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BookCopyCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await _bookCopyApiService.GetStockSummaryAsync(cancellationToken);

        model.Books = stockSummary
            .OrderBy(x => x.KitapAdi)
            .ToList();

        if (model.KitapId <= 0)
        {
            TempData["ErrorMessage"] = "Kitap seçilmelidir.";
            return View(model);
        }

        if (string.IsNullOrWhiteSpace(model.Barkod))
        {
            TempData["ErrorMessage"] = "Barkod zorunludur.";
            return View(model);
        }

        var request = new BookCopyCreateRequest
        {
            KitapId = model.KitapId,
            Barkod = model.Barkod.Trim(),
            DemirbasNo = string.IsNullOrWhiteSpace(model.DemirbasNo)
                ? null
                : model.DemirbasNo.Trim(),
            Aciklama = string.IsNullOrWhiteSpace(model.Aciklama)
                ? null
                : model.Aciklama.Trim()
        };

        var result = await _bookCopyApiService.CreateAsync(
            request,
            cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kitap kopyası oluşturulamadı. Barkod daha önce kullanılmış olabilir.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kitap kopyası başarıyla eklendi.";

        return RedirectToAction(nameof(Index), new { kitapId = model.KitapId });
    }

    private static string GenerateBarcode(int kitapId)
    {
        return $"BK-{kitapId}-{DateTime.Now:yyyyMMddHHmmss}";
    }
}