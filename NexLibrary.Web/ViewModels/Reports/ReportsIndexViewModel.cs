using NexLibrary.Contracts.BookCopies;
using NexLibrary.Contracts.Books;
using NexLibrary.Contracts.Loans;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Web.ViewModels.Reports;

public sealed class ReportsIndexViewModel
{
    public string? Search { get; set; }

    public string? LoanStatus { get; set; } = "Tumu";

    public List<BookListResponse> Books { get; set; } = new();

    public List<MemberListResponse> Members { get; set; } = new();

    public List<LoanListResponse> Loans { get; set; } = new();

    public List<BookCopyStockSummaryResponse> StockSummary { get; set; } = new();

    public List<string> LoanStatuses { get; set; } = new()
    {
        "Tumu",
        "Oduncte",
        "Gecikti",
        "IadeEdildi",
        "IptalEdildi"
    };
}