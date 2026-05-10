using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class UyeConfiguration : IEntityTypeConfiguration<Uye>
{
    public void Configure(EntityTypeBuilder<Uye> builder)
    {
        builder.ToTable("Uyeler");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("UyeId");

        builder.Property(x => x.UyeAdiSoyadi)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.UyeAdiSoyadi);
    }
}