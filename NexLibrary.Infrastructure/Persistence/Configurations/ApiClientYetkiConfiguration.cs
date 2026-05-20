using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class ApiClientYetkiConfiguration : IEntityTypeConfiguration<ApiClientYetki>
{
    public void Configure(EntityTypeBuilder<ApiClientYetki> builder)
    {
        builder.ToTable("ApiClientYetkileri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ApiClientYetkiId");

        builder.Property(x => x.ApiClientId)
            .IsRequired();

        builder.Property(x => x.YetkiTanimiId)
            .IsRequired();

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => new { x.ApiClientId, x.YetkiTanimiId })
            .IsUnique();

        builder.HasOne(x => x.ApiClient)
            .WithMany(x => x.ApiClientYetkileri)
            .HasForeignKey(x => x.ApiClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.YetkiTanimi)
            .WithMany(x => x.ApiClientYetkileri)
            .HasForeignKey(x => x.YetkiTanimiId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}