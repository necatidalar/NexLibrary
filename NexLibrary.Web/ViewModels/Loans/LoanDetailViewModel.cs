using NexLibrary.Contracts.Loans;

namespace NexLibrary.Web.ViewModels.Loans;

public sealed class LoanDetailViewModel
{
    public LoanDetailResponse Loan { get; set; } = new();
}