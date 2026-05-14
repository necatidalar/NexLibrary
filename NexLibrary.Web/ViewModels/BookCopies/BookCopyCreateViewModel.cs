using NexLibrary.Contracts.BookCopies;

namespace NexLibrary.Web.ViewModels.BookCopies;

public sealed class BookCopyCreateViewModel
{
    public int KitapId { get; set; }

    public string Barkod { get; set; } = string.Empty;

    public string? DemirbasNo { get; set; }

    public string? Aciklama { get; set; }

    public List<BookCopyStockSummaryResponse> Books { get; set; } = new();
}