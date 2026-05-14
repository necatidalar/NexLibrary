using Microsoft.AspNetCore.Mvc;
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
}