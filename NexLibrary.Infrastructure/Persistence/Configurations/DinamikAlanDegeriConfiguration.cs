using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class DinamikAlanDegeriConfiguration : IEntityTypeConfiguration<DinamikAlanDegeri>
{
    public void Configure(EntityTypeBuilder<DinamikAlanDegeri> builder)
    {
        builder.ToTable("DinamikAlanDegerleri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("DinamikAlanDegeriId");

        builder.Property(x => x.ModulKodu)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.KayitId)
            .IsRequired();

        builder.Property(x => x.FormAlaniId)
            .IsRequired();

        builder.Property(x => x.DegerMetin)
            .HasMaxLength(4000);

        builder.Property(x => x.DegerSayi)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DegerTarih);

        builder.Property(x => x.DegerBool);

        builder.Property(x => x.DegerJson)
            .HasMaxLength(4000);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => new { x.ModulKodu, x.KayitId });

        builder.HasIndex(x => new { x.ModulKodu, x.KayitId, x.FormAlaniId })
            .IsUnique();

        builder.HasOne(x => x.FormAlani)
            .WithMany(x => x.DinamikAlanDegerleri)
            .HasForeignKey(x => x.FormAlaniId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}