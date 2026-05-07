namespace NexLibrary.Contracts.DynamicForms;

public sealed class FormDesignResponse
{
    public string ModulKodu { get; set; } = string.Empty;

    public string ModulAdi { get; set; } = string.Empty;

    public List<FormFieldResponse> Alanlar { get; set; } = new();
}