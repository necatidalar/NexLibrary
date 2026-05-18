using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations;

public partial class AddMissingPermissionDefinitions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var createdAt = new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);

        migrationBuilder.InsertData(
            table: "YetkiTanimlari",
            columns: new[]
            {
                "YetkiTanimiId",
                "Aciklama",
                "AktifMi",
                "GuncellemeTarihi",
                "GuncelleyenKullaniciId",
                "MenuYetkisiMi",
                "ModulKodu",
                "OlusturanKullaniciId",
                "OlusturmaTarihi",
                "SiraNo",
                "YetkiAdi",
                "YetkiKodu"
            },
            values: new object[,]
            {
                { 62, null, true, null, null, false, "FormFields", null, createdAt, 62, "Form Alanı Ekle", "FormFields.Create" },
                { 63, null, true, null, null, false, "FormFields", null, createdAt, 63, "Form Alanı Yönet", "FormFields.Manage" },
                { 74, null, true, null, null, false, "Users", null, createdAt, 74, "Kullanıcı Sil", "Users.Delete" },
                { 90, null, true, null, null, true, "AuditLogs", null, createdAt, 90, "Audit Kayıtlarını Görüntüle", "AuditLogs.View" }
            });

        migrationBuilder.InsertData(
            table: "RolYetkileri",
            columns: new[]
            {
                "RolYetkiId",
                "AktifMi",
                "GuncellemeTarihi",
                "GuncelleyenKullaniciId",
                "OlusturanKullaniciId",
                "OlusturmaTarihi",
                "RolId",
                "YetkiTanimiId"
            },
            values: new object[,]
            {
                { 51, true, null, null, null, createdAt, 1, 62 },
                { 52, true, null, null, null, createdAt, 1, 63 },
                { 53, true, null, null, null, createdAt, 1, 74 },
                { 54, true, null, null, null, createdAt, 1, 90 }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RolYetkileri",
            keyColumn: "RolYetkiId",
            keyValues: new object[]
            {
                51,
                52,
                53,
                54
            });

        migrationBuilder.DeleteData(
            table: "YetkiTanimlari",
            keyColumn: "YetkiTanimiId",
            keyValues: new object[]
            {
                62,
                63,
                74,
                90
            });
    }
}