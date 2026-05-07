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
    }
}