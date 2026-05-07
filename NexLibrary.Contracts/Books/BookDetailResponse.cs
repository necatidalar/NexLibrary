using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Books;

public sealed class BookDetailResponse
{
    public int Id { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public List<DynamicFieldValueResponse> DinamikAlanlar { get; set; } = new();
}