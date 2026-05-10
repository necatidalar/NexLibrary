using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Members;

public sealed class MemberUpdateRequest
{
    public int Id { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public bool AktifMi { get; set; } = true;

    public List<DynamicFieldValueRequest> DinamikAlanlar { get; set; } = new();
}