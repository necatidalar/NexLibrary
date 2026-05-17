using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Roller");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RolId");

        builder.Property(x => x.RolKodu)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.RolAdi)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Aciklama)
            .HasMaxLength(500);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.RolKodu)
            .IsUnique();

        builder.HasData(
            new Rol
            {
                Id = 1,
                RolKodu = "ADMIN",
                RolAdi = "Admin",
                Aciklama = "Sistemde tüm işlemleri yapabilir.",
                AktifMi = true,
                OlusturmaTarihi = new DateTime(2026, 1, 1)
            },
            new Rol
            {
                Id = 2,
                RolKodu = "PERSONEL",
                RolAdi = "Personel",
                Aciklama = "Kitap, üye, ödünç ve iade işlemlerini yapabilir.",
                AktifMi = true,
                OlusturmaTarihi = new DateTime(2026, 1, 1)
            },
            new Rol
            {
                Id = 3,
                RolKodu = "GORUNTULEYICI",
                RolAdi = "Görüntüleyici",
                Aciklama = "Sadece listeleme, detay ve rapor görüntüleme işlemlerini yapabilir.",
                AktifMi = true,
                OlusturmaTarihi = new DateTime(2026, 1, 1)
            }
        );
    }
}