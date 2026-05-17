namespace NexLibrary.Contracts.Permissions;

public sealed class RolePermissionItemResponse
{
    public int YetkiTanimiId { get; set; }

    public string ModulKodu { get; set; } = string.Empty;

    public string YetkiKodu { get; set; } = string.Empty;

    public string YetkiAdi { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public bool MenuYetkisiMi { get; set; }

    public int SiraNo { get; set; }

    public bool SeciliMi { get; set; }
}