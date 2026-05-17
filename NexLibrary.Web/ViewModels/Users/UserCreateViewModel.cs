using NexLibrary.Contracts.Users;

namespace NexLibrary.Web.ViewModels.Users;

public sealed class UserCreateViewModel
{
    public string KullaniciAdi { get; set; } = string.Empty;

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public string Sifre { get; set; } = string.Empty;

    public int RolId { get; set; }

    public bool AktifMi { get; set; } = true;

    public List<RoleResponse> Roles { get; set; } = new();
}