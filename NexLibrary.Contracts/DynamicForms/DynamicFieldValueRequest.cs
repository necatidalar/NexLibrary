namespace NexLibrary.Contracts.DynamicForms;

public sealed class DynamicFieldValueRequest
{
    public string AlanKodu { get; set; } = string.Empty;

    public string? Deger { get; set; }
}