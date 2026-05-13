namespace NexLibrary.Contracts.BookCopies;

public sealed class BookCopyStockSummaryResponse
{
    public int KitapId { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public int ToplamKopya { get; set; }

    public int Musait { get; set; }

    public int Oduncte { get; set; }

    public int Gecikti { get; set; }

    public int Kayip { get; set; }

    public int Hasarli { get; set; }

    public int Pasif { get; set; }
}