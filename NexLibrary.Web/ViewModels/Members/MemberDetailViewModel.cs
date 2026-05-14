using NexLibrary.Contracts.Members;
using NexLibrary.Contracts.Loans;

namespace NexLibrary.Web.ViewModels.Members;

public sealed class MemberDetailViewModel
{
    public MemberDetailResponse Member { get; set; } = new();

    public List<LoanListResponse> Loans { get; set; } = new();
}