using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class KullaniciConfiguration : IEntityTypeConfiguration<Kullanici>
{
    public void Configure(EntityTypeBuilder<Kullanici> builder)
    {
        builder.ToTable("Kullanicilar");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("KullaniciId");

        builder.Property(x => x.KullaniciAdi)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.AdSoyad)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Eposta)
            .HasMaxLength(150);

        builder.Property(x => x.Telefon)
            .HasMaxLength(30);

        builder.Property(x => x.SifreHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.SifreSalt)
            .HasMaxLength(500);

        builder.Property(x => x.SonGirisTarihi);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.KullaniciAdi)
            .IsUnique();

        builder.HasIndex(x => x.Eposta);
    }
}