namespace NexLibrary.Contracts.DynamicForms;

public sealed class FormFieldResponse
{
    public int Id { get; set; }

    public string ModulKodu { get; set; } = string.Empty;

    public string AlanKodu { get; set; } = string.Empty;

    public string AlanAdi { get; set; } = string.Empty;

    public string AlanTipi { get; set; } = string.Empty;

    public int? MinimumKarakter { get; set; }

    public int? MaksimumKarakter { get; set; }

    public bool ZorunluMu { get; set; }

    public bool BenzersizMi { get; set; }

    public string? VarsayilanDeger { get; set; }

    public string? Aciklama { get; set; }

    public string? Placeholder { get; set; }

    public int SiraNo { get; set; }

    public bool FormdaGorunsunMu { get; set; }

    public bool ListedeGorunsunMu { get; set; }

    public bool AramadaGorunsunMu { get; set; }

    public bool DetaydaGorunsunMu { get; set; }

    public bool HizliKayittaGorunsunMu { get; set; }

    public bool SistemAlaniMi { get; set; }

    public bool SilinebilirMi { get; set; }

    public bool TipDegistirilebilirMi { get; set; }

    public bool AktifMi { get; set; }
}