using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Web.ViewModels.Loans;

public sealed class LoansIndexViewModel
{
    public string? Search { get; set; }

    public bool OverdueOnly { get; set; }

    public string? Durum { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public PagedResponse<LoanListResponse> Loans { get; set; } = new();

    public List<string> Durumlar { get; set; } = new()
    {
        "Tumu",
        "Oduncte",
        "Gecikti",
        "IadeEdildi",
        "IptalEdildi"
    };

    public List<int> PageSizeOptions { get; set; } = new()
    {
        10,
        20,
        50,
        100
    };
}