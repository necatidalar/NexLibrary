namespace NexLibrary.Contracts.Members;

public sealed class MemberListResponse
{
    public int Id { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public Dictionary<string, string?> DinamikAlanlar { get; set; } = new();
}