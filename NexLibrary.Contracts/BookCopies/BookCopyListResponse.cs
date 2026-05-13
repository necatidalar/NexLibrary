namespace NexLibrary.Contracts.BookCopies;

public sealed class BookCopyListResponse
{
    public int Id { get; set; }

    public int KitapId { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public string Barkod { get; set; } = string.Empty;

    public string? DemirbasNo { get; set; }

    public string Durum { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public bool AktifMi { get; set; }
}