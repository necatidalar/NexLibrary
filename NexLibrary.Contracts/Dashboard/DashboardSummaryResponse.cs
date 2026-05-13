namespace NexLibrary.Contracts.Dashboard;

public sealed class DashboardSummaryResponse
{
    public int ToplamKitap { get; set; }

    public int ToplamUye { get; set; }

    public int ToplamKopya { get; set; }

    public int MusaitKopya { get; set; }

    public int OdunctekiKopya { get; set; }

    public int GecikenKopya { get; set; }

    public int KayipKopya { get; set; }

    public int HasarliKopya { get; set; }

    public int AktifOdunc { get; set; }

    public int GecikenOdunc { get; set; }

    public int BugunIadeEdilen { get; set; }

    public int Son7GunOdunc { get; set; }

    public int Son7GunIade { get; set; }

    public List<RecentLoanSummaryResponse> SonOduncler { get; set; } = new();
}