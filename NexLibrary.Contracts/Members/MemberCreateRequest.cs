using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Members;

public sealed class MemberCreateRequest
{
    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public List<DynamicFieldValueRequest> DinamikAlanlar { get; set; } = new();
}