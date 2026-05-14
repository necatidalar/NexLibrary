using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.ViewModels.Books;

public sealed class BookEditViewModel
{
    public int Id { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public List<FormFieldResponse> Fields { get; set; } = new();

    public Dictionary<string, string?> DynamicValues { get; set; } = new();
}