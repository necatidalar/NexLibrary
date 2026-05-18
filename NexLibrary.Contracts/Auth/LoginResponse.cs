namespace NexLibrary.Contracts.Auth;

public sealed class LoginResponse
{
    public int KullaniciId { get; set; }

    public string KullaniciAdi { get; set; } = string.Empty;

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public List<string> Roller { get; set; } = new();

    public List<string> Yetkiler { get; set; } = new();

    public string AccessToken { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }
}