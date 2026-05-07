using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Contracts.Books;

public sealed class BookUpdateRequest
{
    public int Id { get; set; }

    public string KitapAdi { get; set; } = string.Empty;

    public bool AktifMi { get; set; } = true;

    public List<DynamicFieldValueRequest> DinamikAlanlar { get; set; } = new();
}