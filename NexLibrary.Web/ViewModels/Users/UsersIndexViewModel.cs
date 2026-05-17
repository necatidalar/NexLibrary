using NexLibrary.Contracts.Common;
using NexLibrary.Contracts.Users;

namespace NexLibrary.Web.ViewModels.Users;

public sealed class UsersIndexViewModel
{
    public string? Search { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public PagedResponse<UserListResponse> Users { get; set; } = new();

    public List<int> PageSizeOptions { get; set; } = new()
    {
        10,
        20,
        50,
        100
    };
}