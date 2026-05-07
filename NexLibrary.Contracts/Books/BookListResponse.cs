namespace NexLibrary.Contracts.Books;

public sealed class BookListResponse
{
    public int Id { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public Dictionary<string, string?> DinamikAlanlar { get; set; } = new();
}