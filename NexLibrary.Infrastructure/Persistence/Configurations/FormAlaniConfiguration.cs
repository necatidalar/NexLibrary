using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class FormAlaniConfiguration : IEntityTypeConfiguration<FormAlani>
{
    public void Configure(EntityTypeBuilder<FormAlani> builder)
    {
        builder.ToTable("FormAlanlari");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("FormAlaniId");

        builder.Property(x => x.ModulKodu)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.AlanKodu)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.AlanAdi)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.AlanTipi)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.MinimumKarakter);

        builder.Property(x => x.MaksimumKarakter);

        builder.Property(x => x.ZorunluMu)
            .IsRequired();

        builder.Property(x => x.BenzersizMi)
            .IsRequired();

        builder.Property(x => x.VarsayilanDeger)
            .HasMaxLength(500);

        builder.Property(x => x.Aciklama)
            .HasMaxLength(500);

        builder.Property(x => x.Placeholder)
            .HasMaxLength(250);

        builder.Property(x => x.SiraNo)
            .IsRequired();

        builder.Property(x => x.FormdaGorunsunMu)
            .IsRequired();

        builder.Property(x => x.ListedeGorunsunMu)
            .IsRequired();

        builder.Property(x => x.AramadaGorunsunMu)
            .IsRequired();

        builder.Property(x => x.DetaydaGorunsunMu)
            .IsRequired();

        builder.Property(x => x.HizliKayittaGorunsunMu)
            .IsRequired();

        builder.Property(x => x.SistemAlaniMi)
            .IsRequired();

        builder.Property(x => x.SilinebilirMi)
            .IsRequired();

        builder.Property(x => x.TipDegistirilebilirMi)
            .IsRequired();

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => new { x.ModulKodu, x.AlanKodu })
            .IsUnique();

        builder.HasIndex(x => new { x.ModulKodu, x.SiraNo });

        builder.HasMany(x => x.DinamikAlanDegerleri)
            .WithOne(x => x.FormAlani)
            .HasForeignKey(x => x.FormAlaniId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}