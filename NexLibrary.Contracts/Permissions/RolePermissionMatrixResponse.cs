namespace NexLibrary.Contracts.Permissions;

public sealed class RolePermissionMatrixResponse
{
    public int RolId { get; set; }

    public string RolKodu { get; set; } = string.Empty;

    public string RolAdi { get; set; } = string.Empty;

    public List<RolePermissionItemResponse> Yetkiler { get; set; } = new();
}