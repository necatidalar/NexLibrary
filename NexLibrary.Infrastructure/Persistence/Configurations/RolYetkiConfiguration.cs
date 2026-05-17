using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class RolYetkiConfiguration : IEntityTypeConfiguration<RolYetki>
{
    public void Configure(EntityTypeBuilder<RolYetki> builder)
    {
        builder.ToTable("RolYetkileri");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("RolYetkiId");

        builder.Property(x => x.RolId)
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

        builder.HasIndex(x => new { x.RolId, x.YetkiTanimiId })
            .IsUnique();

        builder.HasOne(x => x.Rol)
            .WithMany(x => x.RolYetkileri)
            .HasForeignKey(x => x.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.YetkiTanimi)
            .WithMany(x => x.RolYetkileri)
            .HasForeignKey(x => x.YetkiTanimiId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(GetDefaultRolePermissions());
    }

    private static List<RolYetki> GetDefaultRolePermissions()
    {
        var date = new DateTime(2026, 1, 1);

        var adminPermissions = new[]
        {
            1,
            10, 11, 12, 13,
            20, 21, 22, 23,
            30, 31, 32, 33,
            40, 41, 42, 43,
            50, 51, 52,
            60, 61,
            70, 71, 72, 73,
            80, 81
        };

        var personelPermissions = new[]
        {
            1,
            10, 11, 12,
            20, 21, 22,
            30, 31, 32,
            40, 41, 42,
            50, 51, 52
        };

        var goruntuleyiciPermissions = new[]
        {
            1,
            10,
            20,
            30,
            40,
            50
        };

        var list = new List<RolYetki>();
        var id = 1;

        foreach (var permissionId in adminPermissions)
        {
            list.Add(new RolYetki
            {
                Id = id++,
                RolId = 1,
                YetkiTanimiId = permissionId,
                AktifMi = true,
                OlusturmaTarihi = date
            });
        }

        foreach (var permissionId in personelPermissions)
        {
            list.Add(new RolYetki
            {
                Id = id++,
                RolId = 2,
                YetkiTanimiId = permissionId,
                AktifMi = true,
                OlusturmaTarihi = date
            });
        }

        foreach (var permissionId in goruntuleyiciPermissions)
        {
            list.Add(new RolYetki
            {
                Id = id++,
                RolId = 3,
                YetkiTanimiId = permissionId,
                AktifMi = true,
                OlusturmaTarihi = date
            });
        }

        return list;
    }
}