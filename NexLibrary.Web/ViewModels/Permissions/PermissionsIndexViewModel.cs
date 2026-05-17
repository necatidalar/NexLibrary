namespace NexLibrary.Web.ViewModels.Permissions;

public sealed class PermissionsIndexViewModel
{
    public int SelectedRoleId { get; set; }

    public string SelectedRoleName { get; set; } = string.Empty;

    public string SelectedRoleCode { get; set; } = string.Empty;

    public List<PermissionRoleOptionViewModel> Roles { get; set; } = new();

    public List<PermissionGroupViewModel> Groups { get; set; } = new();

    public List<int> SelectedPermissionIds { get; set; } = new();
}