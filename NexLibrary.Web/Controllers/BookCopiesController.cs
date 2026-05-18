using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.BookCopies;

namespace NexLibrary.Web.Controllers;

public sealed class BookCopiesController : Controller
{
    private readonly BookCopyApiService _bookCopyApiService;
    private readonly ILogger<BookCopiesController> _logger;

    public BookCopiesController(
        BookCopyApiService bookCopyApiService,
        ILogger<BookCopiesController> logger)
    {
        _bookCopyApiService = bookCopyApiService;
        _logger = logger;
    }

    [PermissionAuthorize(PermissionCodes.BookCopiesView)]
    public async Task<IActionResult> Index(
        int? kitapId = null,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await GetSafeStockSummaryAsync(cancellationToken);

        var model = new BookCopiesIndexViewModel
        {
            SelectedBookId = kitapId,
            StockSummary = stockSummary,
            Copies = new List<BookCopyListResponse>()
        };

        if (kitapId.HasValue && kitapId.Value > 0)
        {
            var selectedBook = stockSummary.FirstOrDefault(x => x.KitapId == kitapId.Value);

            model.SelectedBookName = selectedBook?.KitapAdi;

            try
            {
                model.Copies = await _bookCopyApiService.GetByBookIdAsync(
                    kitapId.Value,
                    cancellationToken) ?? new List<BookCopyListResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kitap kopyaları alınırken hata oluştu. KitapId: {KitapId}",
                    kitapId.Value);

                TempData["ErrorMessage"] = "Kitap kopyaları alınamadı.";
                model.Copies = new List<BookCopyListResponse>();
            }
        }

        return View(model);
    }

    [HttpGet]
    [PermissionAuthorize(PermissionCodes.BookCopiesCreate)]
    public async Task<IActionResult> Create(
        int? kitapId = null,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await GetSafeStockSummaryAsync(cancellationToken);

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
    [PermissionAuthorize(PermissionCodes.BookCopiesCreate)]
    public async Task<IActionResult> Create(
        BookCopyCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        var stockSummary = await GetSafeStockSummaryAsync(cancellationToken);

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

        var result = await CreateBookCopySafeAsync(request, cancellationToken);

        if (result is null)
        {
            TempData["ErrorMessage"] = "Kitap kopyası oluşturulamadı. Barkod daha önce kullanılmış olabilir.";
            return View(model);
        }

        TempData["SuccessMessage"] = "Kitap kopyası başarıyla eklendi.";

        return RedirectToAction(nameof(Index), new { kitapId = model.KitapId });
    }

    [PermissionAuthorize(PermissionCodes.BookCopiesView)]
    public async Task<IActionResult> Details(
        int kitapId,
        int copyId,
        CancellationToken cancellationToken = default)
    {
        if (kitapId <= 0 || copyId <= 0)
        {
            TempData["ErrorMessage"] = "Geçersiz kitap kopyası bilgisi.";
            return RedirectToAction(nameof(Index));
        }

        List<BookCopyListResponse> copies;

        try
        {
            copies = await _bookCopyApiService.GetByBookIdAsync(
                kitapId,
                cancellationToken) ?? new List<BookCopyListResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Kitap kopyası detayı alınırken hata oluştu. KitapId: {KitapId}, CopyId: {CopyId}",
                kitapId,
                copyId);

            TempData["ErrorMessage"] = "Kitap kopyası bilgisi alınamadı.";
            return RedirectToAction(nameof(Index), new { kitapId });
        }

        var copy = copies.FirstOrDefault(x => x.Id == copyId);

        if (copy is null)
        {
            TempData["ErrorMessage"] = "Kitap kopyası bulunamadı.";
            return RedirectToAction(nameof(Index), new { kitapId });
        }

        var model = new BookCopyDetailViewModel
        {
            Copy = copy,
            OtherCopies = copies
                .Where(x => x.Id != copyId)
                .OrderBy(x => x.Barkod)
                .ToList()
        };

        return View(model);
    }

    private async Task<List<BookCopyStockSummaryResponse>> GetSafeStockSummaryAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            return await _bookCopyApiService.GetStockSummaryAsync(cancellationToken)
                ?? new List<BookCopyStockSummaryResponse>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitap kopyası stok özeti alınamadı.");
            TempData["ErrorMessage"] = "Kitap listesi alınamadı.";
            return new List<BookCopyStockSummaryResponse>();
        }
    }

    private async Task<object?> CreateBookCopySafeAsync(
        BookCopyCreateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _bookCopyApiService.CreateAsync(
                request,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitap kopyası oluşturulurken hata oluştu.");
            return null;
        }
    }

    private static string GenerateBarcode(int kitapId)
    {
        return $"BK-{kitapId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}