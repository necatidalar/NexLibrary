using NexLibrary.Contracts.DynamicForms;

namespace NexLibrary.Web.ViewModels.FormFields;

public sealed class FormFieldsIndexViewModel
{
    public string ModulKodu { get; set; } = "Kitaplar";

    public List<string> Modules { get; set; } = new()
    {
        "Kitaplar",
        "Uyeler",
        "Personeller",
        "Oduncler",
        "Iadeler"
    };

    public List<FormFieldResponse> Fields { get; set; } = new();

    public FormFieldCreateRequest NewField { get; set; } = new()
    {
        ModulKodu = "Kitaplar",
        AlanTipi = "Metin",
        MinimumKarakter = null,
        MaksimumKarakter = 200,
        ZorunluMu = false,
        BenzersizMi = false,
        SiraNo = 10,
        FormdaGorunsunMu = true,
        ListedeGorunsunMu = true,
        AramadaGorunsunMu = true,
        DetaydaGorunsunMu = true,
        HizliKayittaGorunsunMu = true
    };

    public List<string> FieldTypes { get; set; } = new()
    {
        "Metin",
        "UzunMetin",
        "Sayi",
        "OndalikliSayi",
        "Tarih",
        "TarihSaat",
        "EvetHayir",
        "Liste",
        "CokluListe",
        "Telefon",
        "Eposta",
        "Para",
        "Barkod"
    };
}