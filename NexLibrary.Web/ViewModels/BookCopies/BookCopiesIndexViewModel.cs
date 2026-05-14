using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Web.ViewModels.BookCopies;

public sealed class BookCopiesIndexViewModel
{
    public int? SelectedBookId { get; set; }

    public string? SelectedBookName { get; set; }

    public List<BookCopyStockSummaryResponse> StockSummary { get; set; } = new();

    public List<BookCopyListResponse> Copies { get; set; } = new();
}