namespace NexLibrary.Contracts.Users;

public sealed class RoleResponse
{
    public int Id { get; set; }

    public string RolKodu { get; set; } = string.Empty;

    public string RolAdi { get; set; } = string.Empty;

    public string? Aciklama { get; set; }

    public bool AktifMi { get; set; }
}