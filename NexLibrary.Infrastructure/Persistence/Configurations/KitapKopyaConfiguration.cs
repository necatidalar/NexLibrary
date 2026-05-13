using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class KitapKopyaConfiguration : IEntityTypeConfiguration<KitapKopya>
{
    public void Configure(EntityTypeBuilder<KitapKopya> builder)
    {
        builder.ToTable("KitapKopyalari");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("KitapKopyaId");

        builder.Property(x => x.KitapId)
            .IsRequired();

        builder.Property(x => x.Barkod)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.DemirbasNo)
            .HasMaxLength(100);

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

        builder.HasIndex(x => x.KitapId);

        builder.HasIndex(x => x.Barkod)
            .IsUnique();

        builder.HasIndex(x => x.Durum);
    }
}