using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Web.ViewModels.Reports;

public sealed class ReportsIndexViewModel
{
    public string ReportType { get; set; } = "Loans";

    public string? Search { get; set; }

    public string? LoanStatus { get; set; } = "Tumu";

    public string? ActiveStatus { get; set; } = "Tumu";

    public string? StockStatus { get; set; } = "Tumu";

    public DateTime? DateFrom { get; set; }

    public DateTime? DateTo { get; set; }

    public bool Detailed { get; set; }

    public List<BookListResponse> Books { get; set; } = new();

    public List<MemberListResponse> Members { get; set; } = new();

    public List<LoanListResponse> Loans { get; set; } = new();

    public List<BookCopyStockSummaryResponse> StockSummary { get; set; } = new();

    public List<ReportSelectOption> ReportTypes { get; set; } = new()
    {
        new ReportSelectOption("Summary", "Özet Rapor"),
        new ReportSelectOption("Books", "Kitap Raporu"),
        new ReportSelectOption("Members", "Üye Raporu"),
        new ReportSelectOption("Loans", "Ödünç Raporu"),
        new ReportSelectOption("Overdue", "Geciken Kitaplar Raporu"),
        new ReportSelectOption("Stock", "Kitap Stok Raporu")
    };

    public List<ReportSelectOption> LoanStatuses { get; set; } = new()
    {
        new ReportSelectOption("Tumu", "Tüm Durumlar"),
        new ReportSelectOption("Oduncte", "Ödünçte"),
        new ReportSelectOption("Gecikti", "Gecikti"),
        new ReportSelectOption("IadeEdildi", "İade Edildi"),
        new ReportSelectOption("IptalEdildi", "İptal Edildi")
    };

    public List<ReportSelectOption> ActiveStatuses { get; set; } = new()
    {
        new ReportSelectOption("Tumu", "Tüm Kayıtlar"),
        new ReportSelectOption("Aktif", "Sadece Aktif"),
        new ReportSelectOption("Pasif", "Sadece Pasif")
    };

    public List<ReportSelectOption> StockStatuses { get; set; } = new()
    {
        new ReportSelectOption("Tumu", "Tüm Stoklar"),
        new ReportSelectOption("MusaitVar", "Müsait Stoğu Olanlar"),
        new ReportSelectOption("MusaitYok", "Müsait Stoğu Olmayanlar"),
        new ReportSelectOption("Sorunlu", "Kayıp / Hasarlı / Geciken")
    };

    public string ReportTitle
    {
        get
        {
            return ReportType switch
            {
                "Summary" => "Özet Rapor",
                "Books" => "Kitap Raporu",
                "Members" => "Üye Raporu",
                "Loans" => "Ödünç Raporu",
                "Overdue" => "Geciken Kitaplar Raporu",
                "Stock" => "Kitap Stok Raporu",
                _ => "Rapor"
            };
        }
    }
}

public sealed class ReportSelectOption
{
    public ReportSelectOption(string value, string text)
    {
        Value = value;
        Text = text;
    }

    public string Value { get; set; }

    public string Text { get; set; }
}