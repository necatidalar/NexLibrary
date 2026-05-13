namespace NexLibrary.Contracts.BookCopies;

public sealed class BookCopyCreateRequest
{
    public int KitapId { get; set; }

    public string Barkod { get; set; } = string.Empty;

    public string? DemirbasNo { get; set; }

    public string? Aciklama { get; set; }
}