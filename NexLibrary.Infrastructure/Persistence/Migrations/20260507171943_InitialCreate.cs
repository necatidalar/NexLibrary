using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLoglari",
                columns: table => new
                {
                    AuditLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IslemTuru = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TabloAdi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    KayitId = table.Column<int>(type: "int", nullable: true),
                    EskiDegerJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeniDegerJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    KullaniciId = table.Column<int>(type: "int", nullable: true),
                    IpAdresi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLoglari", x => x.AuditLogId);
                });

            migrationBuilder.CreateTable(
                name: "FormAlanlari",
                columns: table => new
                {
                    FormAlaniId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModulKodu = table.Column<int>(type: "int", nullable: false),
                    AlanKodu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AlanAdi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AlanTipi = table.Column<int>(type: "int", nullable: false),
                    MinimumKarakter = table.Column<int>(type: "int", nullable: true),
                    MaksimumKarakter = table.Column<int>(type: "int", nullable: true),
                    ZorunluMu = table.Column<bool>(type: "bit", nullable: false),
                    BenzersizMi = table.Column<bool>(type: "bit", nullable: false),
                    VarsayilanDeger = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Placeholder = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SiraNo = table.Column<int>(type: "int", nullable: false),
                    FormdaGorunsunMu = table.Column<bool>(type: "bit", nullable: false),
                    ListedeGorunsunMu = table.Column<bool>(type: "bit", nullable: false),
                    AramadaGorunsunMu = table.Column<bool>(type: "bit", nullable: false),
                    DetaydaGorunsunMu = table.Column<bool>(type: "bit", nullable: false),
                    HizliKayittaGorunsunMu = table.Column<bool>(type: "bit", nullable: false),
                    SistemAlaniMi = table.Column<bool>(type: "bit", nullable: false),
                    SilinebilirMi = table.Column<bool>(type: "bit", nullable: false),
                    TipDegistirilebilirMi = table.Column<bool>(type: "bit", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormAlanlari", x => x.FormAlaniId);
                });

            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    KitapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitapAdi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.KitapId);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdSoyad = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Eposta = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SifreHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SifreSalt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SonGirisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Roller",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolKodu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RolAdi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "DinamikAlanDegerleri",
                columns: table => new
                {
                    DinamikAlanDegeriId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModulKodu = table.Column<int>(type: "int", nullable: false),
                    KayitId = table.Column<int>(type: "int", nullable: false),
                    FormAlaniId = table.Column<int>(type: "int", nullable: false),
                    DegerMetin = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DegerSayi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DegerTarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DegerBool = table.Column<bool>(type: "bit", nullable: true),
                    DegerJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DinamikAlanDegerleri", x => x.DinamikAlanDegeriId);
                    table.ForeignKey(
                        name: "FK_DinamikAlanDegerleri_FormAlanlari_FormAlaniId",
                        column: x => x.FormAlaniId,
                        principalTable: "FormAlanlari",
                        principalColumn: "FormAlaniId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciRolleri",
                columns: table => new
                {
                    KullaniciRolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciRolleri", x => x.KullaniciRolId);
                    table.ForeignKey(
                        name: "FK_KullaniciRolleri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KullaniciRolleri_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "FormAlanlari",
                columns: new[] { "FormAlaniId", "Aciklama", "AktifMi", "AlanAdi", "AlanKodu", "AlanTipi", "AramadaGorunsunMu", "BenzersizMi", "DetaydaGorunsunMu", "FormdaGorunsunMu", "GuncellemeTarihi", "GuncelleyenKullaniciId", "HizliKayittaGorunsunMu", "ListedeGorunsunMu", "MaksimumKarakter", "MinimumKarakter", "ModulKodu", "OlusturanKullaniciId", "OlusturmaTarihi", "Placeholder", "SilinebilirMi", "SiraNo", "SistemAlaniMi", "TipDegistirilebilirMi", "VarsayilanDeger", "ZorunluMu" },
                values: new object[] { 1, "Kitabın sistemde görünen ana adıdır. Bu alan zorunludur.", true, "Kitap Adı", "KITAP_ADI", 1, true, false, true, true, null, null, true, true, 200, 1, 1, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Örn: Nutuk", false, 1, true, false, null, true });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_KayitId",
                table: "AuditLoglari",
                column: "KayitId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_KullaniciId",
                table: "AuditLoglari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_OlusturmaTarihi",
                table: "AuditLoglari",
                column: "OlusturmaTarihi");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_TabloAdi",
                table: "AuditLoglari",
                column: "TabloAdi");

            migrationBuilder.CreateIndex(
                name: "IX_DinamikAlanDegerleri_FormAlaniId",
                table: "DinamikAlanDegerleri",
                column: "FormAlaniId");

            migrationBuilder.CreateIndex(
                name: "IX_DinamikAlanDegerleri_ModulKodu_KayitId",
                table: "DinamikAlanDegerleri",
                columns: new[] { "ModulKodu", "KayitId" });

            migrationBuilder.CreateIndex(
                name: "IX_DinamikAlanDegerleri_ModulKodu_KayitId_FormAlaniId",
                table: "DinamikAlanDegerleri",
                columns: new[] { "ModulKodu", "KayitId", "FormAlaniId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormAlanlari_ModulKodu_AlanKodu",
                table: "FormAlanlari",
                columns: new[] { "ModulKodu", "AlanKodu" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormAlanlari_ModulKodu_SiraNo",
                table: "FormAlanlari",
                columns: new[] { "ModulKodu", "SiraNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Kitaplar_KitapAdi",
                table: "Kitaplar",
                column: "KitapAdi");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Eposta",
                table: "Kullanicilar",
                column: "Eposta");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_KullaniciAdi",
                table: "Kullanicilar",
                column: "KullaniciAdi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciRolleri_KullaniciId_RolId",
                table: "KullaniciRolleri",
                columns: new[] { "KullaniciId", "RolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciRolleri_RolId",
                table: "KullaniciRolleri",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Roller_RolKodu",
                table: "Roller",
                column: "RolKodu",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLoglari");

            migrationBuilder.DropTable(
                name: "DinamikAlanDegerleri");

            migrationBuilder.DropTable(
                name: "Kitaplar");

            migrationBuilder.DropTable(
                name: "KullaniciRolleri");

            migrationBuilder.DropTable(
                name: "FormAlanlari");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Roller");
        }
    }
}
