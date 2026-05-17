namespace NexLibrary.Contracts.Permissions;

public sealed class UserPermissionResponse
{
    public int KullaniciId { get; set; }

    public string KullaniciAdi { get; set; } = string.Empty;

    public List<string> Yetkiler { get; set; } = new();
}