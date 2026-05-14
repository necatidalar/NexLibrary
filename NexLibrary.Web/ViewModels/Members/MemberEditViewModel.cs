using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.ViewModels.Members;

public sealed class MemberEditViewModel
{
    public int Id { get; set; }

    public string UyeAdiSoyadi { get; set; } = string.Empty;

    public List<FormFieldResponse> Fields { get; set; } = new();

    public Dictionary<string, string?> DynamicValues { get; set; } = new();
}