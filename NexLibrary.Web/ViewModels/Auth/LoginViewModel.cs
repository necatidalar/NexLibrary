namespace NexLibrary.Web.ViewModels.Auth;

public sealed class LoginViewModel
{
    public string KullaniciAdi { get; set; } = string.Empty;

    public string Sifre { get; set; } = string.Empty;

    public bool BeniHatirla { get; set; }

    public string? ReturnUrl { get; set; }
}