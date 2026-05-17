namespace NexLibrary.Contracts.Users;

public sealed class UserCreateRequest
{
    public string KullaniciAdi { get; set; } = string.Empty;

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public string Sifre { get; set; } = string.Empty;

    public int RolId { get; set; }

    public bool AktifMi { get; set; } = true;
}