using Microsoft.EntityFrameworkCore;
using NexLibrary.Domain.Entities;
using NexLibrary.Domain.Enums;

namespace NexLibrary.Infrastructure.Persistence.Seed;

public static class DefaultFormFieldsSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<FormAlani>().HasData(
            new
            {
                Id = 1,
                AktifMi = true,
                OlusturmaTarihi = seedDate,
                GuncellemeTarihi = (DateTime?)null,
                OlusturanKullaniciId = (int?)null,
                GuncelleyenKullaniciId = (int?)null,

                ModulKodu = ModulKodu.Kitaplar,
                AlanKodu = "KITAP_ADI",
                AlanAdi = "Kitap Adı",
                AlanTipi = AlanTipi.Metin,
                MinimumKarakter = 1,
                MaksimumKarakter = 200,
                ZorunluMu = true,
                BenzersizMi = false,
                VarsayilanDeger = (string?)null,
                Aciklama = "Kitabın sistemde görünen ana adıdır. Bu alan zorunludur.",
                Placeholder = "Örn: Nutuk",
                SiraNo = 1,
                FormdaGorunsunMu = true,
                ListedeGorunsunMu = true,
                AramadaGorunsunMu = true,
                DetaydaGorunsunMu = true,
                HizliKayittaGorunsunMu = true,
                SistemAlaniMi = true,
                SilinebilirMi = false,
                TipDegistirilebilirMi = false
            },
            new
            {
                Id = -2,
                AktifMi = true,
                OlusturmaTarihi = seedDate,
                GuncellemeTarihi = (DateTime?)null,
                OlusturanKullaniciId = (int?)null,
                GuncelleyenKullaniciId = (int?)null,

                ModulKodu = ModulKodu.Uyeler,
                AlanKodu = "UYE_ADI_SOYADI",
                AlanAdi = "Üye Adı Soyadı",
                AlanTipi = AlanTipi.Metin,
                MinimumKarakter = 1,
                MaksimumKarakter = 200,
                ZorunluMu = true,
                BenzersizMi = false,
                VarsayilanDeger = (string?)null,
                Aciklama = "Üyenin sistemde görünen ad soyad bilgisidir. Bu alan zorunludur.",
                Placeholder = "Örn: Ahmet Yılmaz",
                SiraNo = 1,
                FormdaGorunsunMu = true,
                ListedeGorunsunMu = true,
                AramadaGorunsunMu = true,
                DetaydaGorunsunMu = true,
                HizliKayittaGorunsunMu = true,
                SistemAlaniMi = true,
                SilinebilirMi = false,
                TipDegistirilebilirMi = false
            }
        );
    }
}