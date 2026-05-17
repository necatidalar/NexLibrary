using NexLibrary.Contracts.Permissions;

namespace NexLibrary.Web.ViewModels.Permissions;

public sealed class PermissionGroupViewModel
{
    public string ModulKodu { get; set; } = string.Empty;

    public List<RolePermissionItemResponse> Items { get; set; } = new();
}