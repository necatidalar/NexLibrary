using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class OduncConfiguration : IEntityTypeConfiguration<Odunc>
{
    public void Configure(EntityTypeBuilder<Odunc> builder)
    {
        builder.ToTable("Oduncler");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("OduncId");

        builder.Property(x => x.KitapId)
            .IsRequired();

        builder.Property(x => x.UyeId)
            .IsRequired();

        builder.Property(x => x.VerilisTarihi)
            .IsRequired();

        builder.Property(x => x.PlanlananIadeTarihi)
            .IsRequired();

        builder.Property(x => x.IadeTarihi);

        builder.Property(x => x.Durum)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Aciklama)
            .HasMaxLength(500);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasOne(x => x.Kitap)
            .WithMany()
            .HasForeignKey(x => x.KitapId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Uye)
            .WithMany()
            .HasForeignKey(x => x.UyeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.KitapId);

        builder.HasIndex(x => x.UyeId);

        builder.HasIndex(x => x.Durum);

        builder.HasIndex(x => x.PlanlananIadeTarihi);
    }
}