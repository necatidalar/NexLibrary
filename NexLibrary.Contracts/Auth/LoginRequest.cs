namespace NexLibrary.Contracts.Auth;

public sealed class LoginRequest
{
    public string KullaniciAdi { get; set; } = string.Empty;

    public string Sifre { get; set; } = string.Empty;
}