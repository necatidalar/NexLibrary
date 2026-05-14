using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.ViewModels.Books;

public sealed class BookCreateViewModel
{
    public string KitapAdi { get; set; } = string.Empty;

    public List<FormFieldResponse> Fields { get; set; } = new();
}