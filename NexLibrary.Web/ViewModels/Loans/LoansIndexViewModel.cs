using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Web.ViewModels.Loans;

public sealed class LoansIndexViewModel
{
    public string? Search { get; set; }

    public bool OverdueOnly { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public PagedResponse<LoanListResponse> Loans { get; set; } = new();
}