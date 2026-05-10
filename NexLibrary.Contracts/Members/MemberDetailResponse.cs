using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Members;

public sealed class MemberDetailResponse
{
    public int Id { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public bool AktifMi { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public DateTime? GuncellemeTarihi { get; set; }

    public List<DynamicFieldValueResponse> DinamikAlanlar { get; set; } = new();
}