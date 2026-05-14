using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.ViewModels.Members;

public sealed class MemberCreateViewModel
{
    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public List<FormFieldResponse> Fields { get; set; } = new();
}