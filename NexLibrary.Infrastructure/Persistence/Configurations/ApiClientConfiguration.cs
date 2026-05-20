using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class ApiClientConfiguration : IEntityTypeConfiguration<ApiClient>
{
    public void Configure(EntityTypeBuilder<ApiClient> builder)
    {
        builder.ToTable("ApiClients");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ApiClientId");

        builder.Property(x => x.ClientId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ClientName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.ClientSecretHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ClientSecretSalt)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Aciklama)
            .HasMaxLength(500);

        builder.Property(x => x.SonKullanimTarihi);

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.ClientId)
            .IsUnique();
    }
}