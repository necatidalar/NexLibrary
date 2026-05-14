namespace NexLibrary.Web.ViewModels.FormFields;

public sealed class FormFieldsSaveSettingsViewModel
{
    public string ModulKodu { get; set; } = "Kitaplar";

    public List<FormFieldSettingInput> Fields { get; set; } = new();
}