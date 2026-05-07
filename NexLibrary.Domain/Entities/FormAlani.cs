using NexLibrary.Domain.Common;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Domain.Entities
{
    public sealed class FormAlani : BaseEntity
    {
        public ModulKodu ModulKodu { get; set; }

        public string AlanKodu { get; set; } = string.Empty;

        public string AlanAdi { get; set; } = string.Empty;

        public AlanTipi AlanTipi { get; set; }

        public int? MinimumKarakter { get; set; }

        public int? MaksimumKarakter { get; set; }

        public bool ZorunluMu { get; set; }

        public bool BenzersizMi { get; set; }

        public string? VarsayilanDeger { get; set; }

        public string? Aciklama { get; set; }

        public string? Placeholder { get; set; }

        public int SiraNo { get; set; }

        public bool FormdaGorunsunMu { get; set; } = true;

        public bool ListedeGorunsunMu { get; set; } = true;

        public bool AramadaGorunsunMu { get; set; }

        public bool DetaydaGorunsunMu { get; set; } = true;

        public bool HizliKayittaGorunsunMu { get; set; }

        public bool SistemAlaniMi { get; set; }

        public bool SilinebilirMi { get; set; } = true;

        public bool TipDegistirilebilirMi { get; set; } = true;

        public ICollection<DinamikAlanDegeri> DinamikAlanDegerleri { get; set; } = new List<DinamikAlanDegeri>();
    }
}
