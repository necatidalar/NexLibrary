namespace NexLibrary.Contracts.Permissions;

public sealed class RolePermissionUpdateRequest
{
    public int RolId { get; set; }

    public List<int> YetkiTanimiIds { get; set; } = new();
}