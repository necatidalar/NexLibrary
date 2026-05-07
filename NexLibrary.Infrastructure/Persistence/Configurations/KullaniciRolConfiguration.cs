using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class KullaniciRolConfiguration : IEntityTypeConfiguration<KullaniciRol>
{
    public void Configure(EntityTypeBuilder<KullaniciRol> builder)
    {
        builder.ToTable("KullaniciRolleri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("KullaniciRolId");

        builder.Property(x => x.KullaniciId)
            .IsRequired();

        builder.Property(x => x.RolId)
            .IsRequired();

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => new { x.KullaniciId, x.RolId })
            .IsUnique();

        builder.HasOne(x => x.Kullanici)
            .WithMany(x => x.KullaniciRolleri)
            .HasForeignKey(x => x.KullaniciId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Rol)
            .WithMany(x => x.KullaniciRolleri)
            .HasForeignKey(x => x.RolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}