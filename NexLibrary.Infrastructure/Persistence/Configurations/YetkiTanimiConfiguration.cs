using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexLibrary.Domain.Entities;

namespace NexLibrary.Infrastructure.Persistence.Configurations;

public sealed class YetkiTanimiConfiguration : IEntityTypeConfiguration<YetkiTanimi>
{
    public void Configure(EntityTypeBuilder<YetkiTanimi> builder)
    {
        builder.ToTable("YetkiTanimlari");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("YetkiTanimiId");

        builder.Property(x => x.ModulKodu)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.YetkiKodu)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.YetkiAdi)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Aciklama)
            .HasMaxLength(500);

        builder.Property(x => x.MenuYetkisiMi)
            .IsRequired();

        builder.Property(x => x.SiraNo)
            .IsRequired();

        builder.Property(x => x.AktifMi)
            .IsRequired();

        builder.Property(x => x.OlusturmaTarihi)
            .IsRequired();

        builder.Property(x => x.GuncellemeTarihi);

        builder.Property(x => x.OlusturanKullaniciId);

        builder.Property(x => x.GuncelleyenKullaniciId);

        builder.HasIndex(x => x.YetkiKodu)
            .IsUnique();

        builder.HasData(GetDefaultPermissions());
    }

    private static List<YetkiTanimi> GetDefaultPermissions()
    {
        var date = new DateTime(2026, 1, 1);

        return new List<YetkiTanimi>
        {
            new() { Id = 1, ModulKodu = "Dashboard", YetkiKodu = "Dashboard.View", YetkiAdi = "Dashboard Görüntüle", MenuYetkisiMi = true, SiraNo = 1, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 10, ModulKodu = "Books", YetkiKodu = "Books.View", YetkiAdi = "Kitapları Görüntüle", MenuYetkisiMi = true, SiraNo = 10, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 11, ModulKodu = "Books", YetkiKodu = "Books.Create", YetkiAdi = "Kitap Ekle", MenuYetkisiMi = false, SiraNo = 11, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 12, ModulKodu = "Books", YetkiKodu = "Books.Edit", YetkiAdi = "Kitap Düzenle", MenuYetkisiMi = false, SiraNo = 12, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 13, ModulKodu = "Books", YetkiKodu = "Books.Delete", YetkiAdi = "Kitap Sil", MenuYetkisiMi = false, SiraNo = 13, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 20, ModulKodu = "Members", YetkiKodu = "Members.View", YetkiAdi = "Üyeleri Görüntüle", MenuYetkisiMi = true, SiraNo = 20, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 21, ModulKodu = "Members", YetkiKodu = "Members.Create", YetkiAdi = "Üye Ekle", MenuYetkisiMi = false, SiraNo = 21, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 22, ModulKodu = "Members", YetkiKodu = "Members.Edit", YetkiAdi = "Üye Düzenle", MenuYetkisiMi = false, SiraNo = 22, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 23, ModulKodu = "Members", YetkiKodu = "Members.Delete", YetkiAdi = "Üye Sil", MenuYetkisiMi = false, SiraNo = 23, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 30, ModulKodu = "Loans", YetkiKodu = "Loans.View", YetkiAdi = "Ödünçleri Görüntüle", MenuYetkisiMi = true, SiraNo = 30, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 31, ModulKodu = "Loans", YetkiKodu = "Loans.Create", YetkiAdi = "Ödünç Ver", MenuYetkisiMi = false, SiraNo = 31, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 32, ModulKodu = "Loans", YetkiKodu = "Loans.Return", YetkiAdi = "İade Al", MenuYetkisiMi = false, SiraNo = 32, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 33, ModulKodu = "Loans", YetkiKodu = "Loans.Cancel", YetkiAdi = "Ödünç İptal Et", MenuYetkisiMi = false, SiraNo = 33, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 40, ModulKodu = "BookCopies", YetkiKodu = "BookCopies.View", YetkiAdi = "Kitap Kopyalarını Görüntüle", MenuYetkisiMi = true, SiraNo = 40, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 41, ModulKodu = "BookCopies", YetkiKodu = "BookCopies.Create", YetkiAdi = "Kitap Kopyası Ekle", MenuYetkisiMi = false, SiraNo = 41, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 42, ModulKodu = "BookCopies", YetkiKodu = "BookCopies.Edit", YetkiAdi = "Kitap Kopyası Düzenle", MenuYetkisiMi = false, SiraNo = 42, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 43, ModulKodu = "BookCopies", YetkiKodu = "BookCopies.Delete", YetkiAdi = "Kitap Kopyası Sil", MenuYetkisiMi = false, SiraNo = 43, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 50, ModulKodu = "Reports", YetkiKodu = "Reports.View", YetkiAdi = "Raporları Görüntüle", MenuYetkisiMi = true, SiraNo = 50, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 51, ModulKodu = "Reports", YetkiKodu = "Reports.Print", YetkiAdi = "Rapor Yazdır", MenuYetkisiMi = false, SiraNo = 51, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 52, ModulKodu = "Reports", YetkiKodu = "Reports.Export", YetkiAdi = "Rapor Dışa Aktar", MenuYetkisiMi = false, SiraNo = 52, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 60, ModulKodu = "FormFields", YetkiKodu = "FormFields.View", YetkiAdi = "Form Alanlarını Görüntüle", MenuYetkisiMi = true, SiraNo = 60, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 61, ModulKodu = "FormFields", YetkiKodu = "FormFields.Edit", YetkiAdi = "Form Alanı Düzenle", MenuYetkisiMi = false, SiraNo = 61, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 70, ModulKodu = "Users", YetkiKodu = "Users.View", YetkiAdi = "Kullanıcıları Görüntüle", MenuYetkisiMi = true, SiraNo = 70, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 71, ModulKodu = "Users", YetkiKodu = "Users.Create", YetkiAdi = "Kullanıcı Ekle", MenuYetkisiMi = false, SiraNo = 71, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 72, ModulKodu = "Users", YetkiKodu = "Users.Edit", YetkiAdi = "Kullanıcı Düzenle", MenuYetkisiMi = false, SiraNo = 72, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 73, ModulKodu = "Users", YetkiKodu = "Users.RoleManage", YetkiAdi = "Kullanıcı Rol Yönet", MenuYetkisiMi = false, SiraNo = 73, AktifMi = true, OlusturmaTarihi = date },

            new() { Id = 80, ModulKodu = "Permissions", YetkiKodu = "Permissions.View", YetkiAdi = "Yetkileri Görüntüle", MenuYetkisiMi = true, SiraNo = 80, AktifMi = true, OlusturmaTarihi = date },
            new() { Id = 81, ModulKodu = "Permissions", YetkiKodu = "Permissions.Manage", YetkiAdi = "Yetki Yönet", MenuYetkisiMi = false, SiraNo = 81, AktifMi = true, OlusturmaTarihi = date }
        };
    }
}