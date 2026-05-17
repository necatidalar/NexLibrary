namespace NexLibrary.Web.ViewModels.FormFields;

public sealed class FormFieldCreateViewModel
{
    public string ModulKodu { get; set; } = "Kitaplar";

    public string AlanKodu { get; set; } = string.Empty;

    public string AlanAdi { get; set; } = string.Empty;

    public string AlanTipi { get; set; } = "Metin";

    public int? MinimumKarakter { get; set; }

    public int? MaksimumKarakter { get; set; } = 200;

    public bool ZorunluMu { get; set; }

    public bool BenzersizMi { get; set; }

    public string? VarsayilanDeger { get; set; }

    public string? Aciklama { get; set; }

    public string? Placeholder { get; set; }

    public int SiraNo { get; set; } = 10;

    public bool FormdaGorunsunMu { get; set; } = true;

    public bool ListedeGorunsunMu { get; set; } = true;

    public bool AramadaGorunsunMu { get; set; } = true;

    public bool DetaydaGorunsunMu { get; set; } = true;

    public bool HizliKayittaGorunsunMu { get; set; } = true;

    public bool AktifMi { get; set; } = true;

    public List<string> Modules { get; set; } = new()
    {
        "Kitaplar",
        "Uyeler",
        "Personeller",
        "Oduncler",
        "Iadeler"
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