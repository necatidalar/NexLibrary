using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersRolesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roller",
                columns: new[] { "RolId", "Aciklama", "AktifMi", "GuncellemeTarihi", "GuncelleyenKullaniciId", "OlusturanKullaniciId", "OlusturmaTarihi", "RolAdi", "RolKodu" },
                values: new object[,]
                {
                    { 1, "Sistemde tüm işlemleri yapabilir.", true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "ADMIN" },
                    { 2, "Kitap, üye, ödünç ve iade işlemlerini yapabilir.", true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel", "PERSONEL" },
                    { 3, "Sadece listeleme, detay ve rapor görüntüleme işlemlerini yapabilir.", true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Görüntüleyici", "GORUNTULEYICI" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roller",
                keyColumn: "RolId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roller",
                keyColumn: "RolId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roller",
                keyColumn: "RolId",
                keyValue: 3);
        }
    }
}
