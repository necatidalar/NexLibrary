namespace NexLibrary.Contracts.Users;

public sealed class UserUpdateRequest
{
    public int Id { get; set; }

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public string? YeniSifre { get; set; }

    public int RolId { get; set; }

    public bool AktifMi { get; set; }
}