using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMembersModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Uyeler",
                columns: table => new
                {
                    UyeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeAdiSoyadi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uyeler", x => x.UyeId);
                });

            migrationBuilder.InsertData(
                table: "FormAlanlari",
                columns: new[] { "FormAlaniId", "Aciklama", "AktifMi", "AlanAdi", "AlanKodu", "AlanTipi", "AramadaGorunsunMu", "BenzersizMi", "DetaydaGorunsunMu", "FormdaGorunsunMu", "GuncellemeTarihi", "GuncelleyenKullaniciId", "HizliKayittaGorunsunMu", "ListedeGorunsunMu", "MaksimumKarakter", "MinimumKarakter", "ModulKodu", "OlusturanKullaniciId", "OlusturmaTarihi", "Placeholder", "SilinebilirMi", "SiraNo", "SistemAlaniMi", "TipDegistirilebilirMi", "VarsayilanDeger", "ZorunluMu" },
                values: new object[] { -2, "Üyenin sistemde görünen ad soyad bilgisidir. Bu alan zorunludur.", true, "Üye Adı Soyadı", "UYE_ADI_SOYADI", 1, true, false, true, true, null, null, true, true, 200, 1, 2, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Örn: Ahmet Yılmaz", false, 1, true, false, null, true });

            migrationBuilder.CreateIndex(
                name: "IX_Uyeler_UyeAdiSoyadi",
                table: "Uyeler",
                column: "UyeAdiSoyadi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Uyeler");

            migrationBuilder.DeleteData(
                table: "FormAlanlari",
                keyColumn: "FormAlaniId",
                keyValue: -2);
        }
    }
}
