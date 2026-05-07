namespace NexLibrary.Contracts.DynamicForms;

public sealed class DynamicFieldValueResponse
{
    public int FormAlaniId { get; set; }

    public string AlanKodu { get; set; } = string.Empty;

    public string AlanAdi { get; set; } = string.Empty;

    public string AlanTipi { get; set; } = string.Empty;

    public string? Deger { get; set; }
}