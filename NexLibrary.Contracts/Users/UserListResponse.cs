namespace NexLibrary.Contracts.Users;

public sealed class UserListResponse
{
    public int Id { get; set; }

    public string KullaniciAdi { get; set; } = string.Empty;

    public string AdSoyad { get; set; } = string.Empty;

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public List<string> Roller { get; set; } = new();
}