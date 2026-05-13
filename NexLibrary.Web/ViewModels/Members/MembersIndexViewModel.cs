using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Members;

namespace NexLibrary.Web.ViewModels.Members;

public sealed class MembersIndexViewModel
{
    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public PagedResponse<MemberListResponse> Members { get; set; } = new();
}