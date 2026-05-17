using NexLibrary.Contracts.Users;

namespace NexLibrary.Web.ViewModels.Users;

public sealed class UserEditViewModel
{
    public int Id { get; set; }

    public string KullaniciAdi { get; set; } = string.Empty;

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public string? YeniSifre { get; set; }

    public int RolId { get; set; }

    public bool AktifMi { get; set; }

    public List<RoleResponse> Roles { get; set; } = new();
}