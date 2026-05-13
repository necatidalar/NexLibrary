namespace NexLibrary.Contracts.Loans;

public sealed class LoanDetailResponse
{
    public int Id { get; set; }

    public int KitapId { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public int UyeId { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; }

    public DateTime? IadeTarihi { get; set; }

    public string Durum { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }
}