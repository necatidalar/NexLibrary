using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLoglari");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("AuditLogId");

        builder.Property(x => x.IslemTuru)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.TabloAdi)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.KayitId);

        builder.Property(x => x.EskiDegerJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.YeniDegerJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(x => x.Aciklama)
            .HasMaxLength(1000);

        builder.Property(x => x.KullaniciId);

        builder.Property(x => x.IpAdresi)
            .HasMaxLength(100);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.TabloAdi);

        builder.HasIndex(x => x.KayitId);

        builder.HasIndex(x => x.KullaniciId);

        builder.HasIndex(x => x.OlusturmaTarihi);
    }
}