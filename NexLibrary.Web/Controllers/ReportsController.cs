using System.Text;
using Microsoft.AspNetCore.Mvc;
using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Members;
using NexLibrary.Contracts.Permissions;
using NexLibrary.Web.Security;
using NexLibrary.Web.Services;
using NexLibrary.Web.ViewModels.Reports;

namespace NexLibrary.Web.Controllers;

public sealed class ReportsController : Controller
{
    private readonly BookApiService _bookApiService;
    private readonly MemberApiService _memberApiService;
    private readonly LoanApiService _loanApiService;
    private readonly BookCopyApiService _bookCopyApiService;

    public ReportsController(
        BookApiService bookApiService,
        MemberApiService memberApiService,
        LoanApiService loanApiService,
        BookCopyApiService bookCopyApiService)
    {
        _bookApiService = bookApiService;
        _memberApiService = memberApiService;
        _loanApiService = loanApiService;
        _bookCopyApiService = bookCopyApiService;
    }

    [PermissionAuthorize(PermissionCodes.ReportsView)]
    public async Task<IActionResult> Index(
        string reportType = "Loans",
        string? search = null,
        string? loanStatus = "Tumu",
        string? activeStatus = "Tumu",
        string? stockStatus = "Tumu",
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        bool detailed = false,
        CancellationToken cancellationToken = default)
    {
        var model = await CreateReportModelAsync(
            reportType,
            search,
            loanStatus,
            activeStatus,
            stockStatus,
            dateFrom,
            dateTo,
            detailed,
            cancellationToken);

        return View(model);
    }

    [PermissionAuthorize(PermissionCodes.ReportsExport)]
    public async Task<IActionResult> ExportExcel(
        string reportType = "Loans",
        string? search = null,
        string? loanStatus = "Tumu",
        string? activeStatus = "Tumu",
        string? stockStatus = "Tumu",
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        bool detailed = false,
        CancellationToken cancellationToken = default)
    {
        var model = await CreateReportModelAsync(
            reportType,
            search,
            loanStatus,
            activeStatus,
            stockStatus,
            dateFrom,
            dateTo,
            detailed,
            cancellationToken);

        var csv = CreateCsv(model);

        var bytes = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(csv))
            .ToArray();

        var fileName = $"{model.ReportTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.csv";

        return File(bytes, "text/csv", fileName);
    }

    private async Task<ReportsIndexViewModel> CreateReportModelAsync(
        string reportType,
        string? search,
        string? loanStatus,
        string? activeStatus,
        string? stockStatus,
        DateTime? dateFrom,
        DateTime? dateTo,
        bool detailed,
        CancellationToken cancellationToken)
    {
        reportType = NormalizeReportType(reportType);

        if (reportType == "Overdue")
        {
            loanStatus = "Gecikti";
        }

        await _loanApiService.MarkOverdueAsync(cancellationToken);

        var model = new ReportsIndexViewModel
        {
            ReportType = reportType,
            Search = search,
            LoanStatus = string.IsNullOrWhiteSpace(loanStatus) ? "Tumu" : loanStatus,
            ActiveStatus = string.IsNullOrWhiteSpace(activeStatus) ? "Tumu" : activeStatus,
            StockStatus = string.IsNullOrWhiteSpace(stockStatus) ? "Tumu" : stockStatus,
            DateFrom = dateFrom,
            DateTo = dateTo,
            Detailed = detailed
        };

        if (reportType is "Summary" or "Books")
        {
            var booksResult = await _bookApiService.GetPagedAsync(
                1,
                1000,
                search,
                cancellationToken);

            model.Books = booksResult?.Items.ToList()
                ?? new List<BookListResponse>();

            model.Books = ApplyBookFilters(model.Books, model.ActiveStatus);
        }

        if (reportType is "Summary" or "Members")
        {
            var membersResult = await _memberApiService.GetPagedAsync(
                1,
                1000,
                search,
                cancellationToken);

            model.Members = membersResult?.Items.ToList()
                ?? new List<MemberListResponse>();

            model.Members = ApplyMemberFilters(model.Members, model.ActiveStatus);
        }

        if (reportType is "Summary" or "Loans" or "Overdue")
        {
            var loansResult = await _loanApiService.GetPagedAsync(
                1,
                1000,
                search,
                cancellationToken);

            model.Loans = loansResult?.Items.ToList()
                ?? new List<LoanListResponse>();

            model.Loans = ApplyLoanFilters(
                model.Loans,
                model.LoanStatus,
                model.DateFrom,
                model.DateTo);
        }

        if (reportType is "Summary" or "Stock")
        {
            model.StockSummary = await _bookCopyApiService.GetStockSummaryAsync(
                cancellationToken);

            model.StockSummary = ApplyStockFilters(
                model.StockSummary,
                search,
                model.StockStatus);
        }

        return model;
    }

    private static string NormalizeReportType(string? reportType)
    {
        if (string.IsNullOrWhiteSpace(reportType))
        {
            return "Loans";
        }

        var allowed = new HashSet<string>
        {
            "Summary",
            "Books",
            "Members",
            "Loans",
            "Overdue",
            "Stock"
        };

        return allowed.Contains(reportType)
            ? reportType
            : "Loans";
    }

    private static List<BookListResponse> ApplyBookFilters(
        List<BookListResponse> books,
        string? activeStatus)
    {
        return activeStatus switch
        {
            "Aktif" => books.Where(x => x.AktifMi).ToList(),
            "Pasif" => books.Where(x => !x.AktifMi).ToList(),
            _ => books
        };
    }

    private static List<MemberListResponse> ApplyMemberFilters(
        List<MemberListResponse> members,
        string? activeStatus)
    {
        return activeStatus switch
        {
            "Aktif" => members.Where(x => x.AktifMi).ToList(),
            "Pasif" => members.Where(x => !x.AktifMi).ToList(),
            _ => members
        };
    }

    private static List<LoanListResponse> ApplyLoanFilters(
        List<LoanListResponse> loans,
        string? loanStatus,
        DateTime? dateFrom,
        DateTime? dateTo)
    {
        if (!string.IsNullOrWhiteSpace(loanStatus) && loanStatus != "Tumu")
        {
            loans = loans
                .Where(x => x.Durum == loanStatus)
                .ToList();
        }

        if (dateFrom.HasValue)
        {
            loans = loans
                .Where(x => x.VerilisTarihi.Date >= dateFrom.Value.Date)
                .ToList();
        }

        if (dateTo.HasValue)
        {
            loans = loans
                .Where(x => x.VerilisTarihi.Date <= dateTo.Value.Date)
                .ToList();
        }

        return loans;
    }

    private static List<BookCopyStockSummaryResponse> ApplyStockFilters(
        List<BookCopyStockSummaryResponse> stockSummary,
        string? search,
        string? stockStatus)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            stockSummary = stockSummary
                .Where(x => x.KitapAdi.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        stockSummary = stockStatus switch
        {
            "MusaitVar" => stockSummary.Where(x => x.Musait > 0).ToList(),
            "MusaitYok" => stockSummary.Where(x => x.Musait <= 0).ToList(),
            "Sorunlu" => stockSummary
                .Where(x => x.Kayip > 0 || x.Hasarli > 0 || x.Gecikti > 0)
                .ToList(),
            _ => stockSummary
        };

        return stockSummary;
    }

    private static string CreateCsv(ReportsIndexViewModel model)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{Csv("Rapor")};{Csv(model.ReportTitle)}");
        sb.AppendLine($"{Csv("Tarih")};{Csv(DateTime.Now.ToString("dd.MM.yyyy HH:mm"))}");
        sb.AppendLine();

        switch (model.ReportType)
        {
            case "Books":
                sb.AppendLine("ID;Kitap Adı;Durum;Oluşturma Tarihi");

                foreach (var book in model.Books)
                {
                    sb.AppendLine(string.Join(";",
                        Csv(book.Id.ToString()),
                        Csv(book.KitapAdi),
                        Csv(book.AktifMi ? "Aktif" : "Pasif"),
                        Csv(book.OlusturmaTarihi.ToString("dd.MM.yyyy"))));
                }

                break;

            case "Members":
                sb.AppendLine("ID;Üye Adı Soyadı;Durum;Oluşturma Tarihi");

                foreach (var member in model.Members)
                {
                    sb.AppendLine(string.Join(";",
                        Csv(member.Id.ToString()),
                        Csv(member.UyeAdiSoyadi),
                        Csv(member.AktifMi ? "Aktif" : "Pasif"),
                        Csv(member.OlusturmaTarihi.ToString("dd.MM.yyyy"))));
                }

                break;

            case "Stock":
                sb.AppendLine("Kitap ID;Kitap Adı;Toplam;Müsait;Ödünçte;Gecikti;Kayıp;Hasarlı;Pasif");

                foreach (var item in model.StockSummary)
                {
                    sb.AppendLine(string.Join(";",
                        Csv(item.KitapId.ToString()),
                        Csv(item.KitapAdi),
                        Csv(item.ToplamKopya.ToString()),
                        Csv(item.Musait.ToString()),
                        Csv(item.Oduncte.ToString()),
                        Csv(item.Gecikti.ToString()),
                        Csv(item.Kayip.ToString()),
                        Csv(item.Hasarli.ToString()),
                        Csv(item.Pasif.ToString())));
                }

                break;

            case "Summary":
                sb.AppendLine("Başlık;Değer");
                sb.AppendLine($"{Csv("Kitap Sayısı")};{Csv(model.Books.Count.ToString())}");
                sb.AppendLine($"{Csv("Üye Sayısı")};{Csv(model.Members.Count.ToString())}");
                sb.AppendLine($"{Csv("Ödünç Sayısı")};{Csv(model.Loans.Count.ToString())}");
                sb.AppendLine($"{Csv("Stokta Kitap Sayısı")};{Csv(model.StockSummary.Count.ToString())}");
                sb.AppendLine($"{Csv("Toplam Kopya")};{Csv(model.StockSummary.Sum(x => x.ToplamKopya).ToString())}");
                sb.AppendLine($"{Csv("Müsait Kopya")};{Csv(model.StockSummary.Sum(x => x.Musait).ToString())}");
                break;

            default:
                sb.AppendLine("ID;Kitap;Barkod;Üye;Veriliş;Planlanan İade;İade;Durum");

                foreach (var loan in model.Loans)
                {
                    sb.AppendLine(string.Join(";",
                        Csv(loan.Id.ToString()),
                        Csv(loan.KitapAdi),
                        Csv(loan.KitapKopyaBarkod ?? "-"),
                        Csv(loan.UyeAdiSoyadi),
                        Csv(loan.VerilisTarihi.ToString("dd.MM.yyyy")),
                        Csv(loan.PlanlananIadeTarihi.ToString("dd.MM.yyyy")),
                        Csv(loan.IadeTarihi?.ToString("dd.MM.yyyy") ?? "-"),
                        Csv(loan.Durum)));
                }

                break;
        }

        return sb.ToString();
    }

    private static string Csv(string value)
    {
        value = value.Replace("\"", "\"\"");
        return $"\"{value}\"";
    }
}