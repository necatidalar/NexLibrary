using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Books;

public sealed class BookCreateRequest
{
    public string KitapAdi { get; set; } = string.Empty;

    public List<DynamicFieldValueRequest> DinamikAlanlar { get; set; } = new();
}