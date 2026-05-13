namespace NexLibrary.Contracts.Dashboard;

public sealed class RecentLoanSummaryResponse
{
    public int Id { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public string? Barkod { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public DateTime VerilisTarihi { get; set; }

    public DateTime PlanlananIadeTarihi { get; set; }

    public DateTime? IadeTarihi { get; set; }

    public string Durum { get; set; } = string.Empty;
}