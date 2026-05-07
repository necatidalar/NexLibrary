using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class KitapConfiguration : IEntityTypeConfiguration<Kitap>
{
    public void Configure(EntityTypeBuilder<Kitap> builder)
    {
        builder.ToTable("Kitaplar");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("KitapId");

        builder.Property(x => x.KitapAdi)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.KitapAdi);

        // Dinamik alan değerleri ModulKodu + KayitId üzerinden yönetilecek.
        // Bu yüzden EF'in KitapId isimli ekstra shadow FK üretmesini engelliyoruz.
        builder.Ignore(x => x.DinamikAlanDegerleri);
    }
}