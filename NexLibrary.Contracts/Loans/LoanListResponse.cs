namespace NexLibrary.Contracts.Loans;

public sealed class LoanListResponse
{
    public int Id { get; set; }

    public int KitapId { get; set; }

    public int? KitapKopyaId { get; set; }

    public string? KitapKopyaBarkod { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public int UyeId { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; }

    public DateTime? IadeTarihi { get; set; }

    public string Durum { get; set; } = string.Empty;

    public bool AktifMi { get; set; }
}