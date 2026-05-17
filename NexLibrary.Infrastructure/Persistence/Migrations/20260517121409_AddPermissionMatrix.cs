using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPermissionMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YetkiTanimlari",
                columns: table => new
                {
                    YetkiTanimiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModulKodu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    YetkiKodu = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    YetkiAdi = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MenuYetkisiMi = table.Column<bool>(type: "bit", nullable: false),
                    SiraNo = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YetkiTanimlari", x => x.YetkiTanimiId);
                });

            migrationBuilder.CreateTable(
                name: "RolYetkileri",
                columns: table => new
                {
                    RolYetkiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    YetkiTanimiId = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolYetkileri", x => x.RolYetkiId);
                    table.ForeignKey(
                        name: "FK_RolYetkileri_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolYetkileri_YetkiTanimlari_YetkiTanimiId",
                        column: x => x.YetkiTanimiId,
                        principalTable: "YetkiTanimlari",
                        principalColumn: "YetkiTanimiId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "YetkiTanimlari",
                columns: new[] { "YetkiTanimiId", "Aciklama", "AktifMi", "GuncellemeTarihi", "GuncelleyenKullaniciId", "MenuYetkisiMi", "ModulKodu", "OlusturanKullaniciId", "OlusturmaTarihi", "SiraNo", "YetkiAdi", "YetkiKodu" },
                values: new object[,]
                {
                    { 1, null, true, null, null, true, "Dashboard", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Dashboard Görüntüle", "Dashboard.View" },
                    { 10, null, true, null, null, true, "Books", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Kitapları Görüntüle", "Books.View" },
                    { 11, null, true, null, null, false, "Books", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Kitap Ekle", "Books.Create" },
                    { 12, null, true, null, null, false, "Books", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Kitap Düzenle", "Books.Edit" },
                    { 13, null, true, null, null, false, "Books", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "Kitap Sil", "Books.Delete" },
                    { 20, null, true, null, null, true, "Members", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Üyeleri Görüntüle", "Members.View" },
                    { 21, null, true, null, null, false, "Members", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, "Üye Ekle", "Members.Create" },
                    { 22, null, true, null, null, false, "Members", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, "Üye Düzenle", "Members.Edit" },
                    { 23, null, true, null, null, false, "Members", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, "Üye Sil", "Members.Delete" },
                    { 30, null, true, null, null, true, "Loans", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Ödünçleri Görüntüle", "Loans.View" },
                    { 31, null, true, null, null, false, "Loans", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, "Ödünç Ver", "Loans.Create" },
                    { 32, null, true, null, null, false, "Loans", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, "İade Al", "Loans.Return" },
                    { 33, null, true, null, null, false, "Loans", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, "Ödünç İptal Et", "Loans.Cancel" },
                    { 40, null, true, null, null, true, "BookCopies", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Kitap Kopyalarını Görüntüle", "BookCopies.View" },
                    { 41, null, true, null, null, false, "BookCopies", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, "Kitap Kopyası Ekle", "BookCopies.Create" },
                    { 42, null, true, null, null, false, "BookCopies", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, "Kitap Kopyası Düzenle", "BookCopies.Edit" },
                    { 43, null, true, null, null, false, "BookCopies", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, "Kitap Kopyası Sil", "BookCopies.Delete" },
                    { 50, null, true, null, null, true, "Reports", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Raporları Görüntüle", "Reports.View" },
                    { 51, null, true, null, null, false, "Reports", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 51, "Rapor Yazdır", "Reports.Print" },
                    { 52, null, true, null, null, false, "Reports", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 52, "Rapor Dışa Aktar", "Reports.Export" },
                    { 60, null, true, null, null, true, "FormFields", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, "Form Alanlarını Görüntüle", "FormFields.View" },
                    { 61, null, true, null, null, false, "FormFields", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 61, "Form Alanı Düzenle", "FormFields.Edit" },
                    { 70, null, true, null, null, true, "Users", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, "Kullanıcıları Görüntüle", "Users.View" },
                    { 71, null, true, null, null, false, "Users", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 71, "Kullanıcı Ekle", "Users.Create" },
                    { 72, null, true, null, null, false, "Users", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, "Kullanıcı Düzenle", "Users.Edit" },
                    { 73, null, true, null, null, false, "Users", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 73, "Kullanıcı Rol Yönet", "Users.RoleManage" },
                    { 80, null, true, null, null, true, "Permissions", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, "Yetkileri Görüntüle", "Permissions.View" },
                    { 81, null, true, null, null, false, "Permissions", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 81, "Yetki Yönet", "Permissions.Manage" }
                });

            migrationBuilder.InsertData(
                table: "RolYetkileri",
                columns: new[] { "RolYetkiId", "AktifMi", "GuncellemeTarihi", "GuncelleyenKullaniciId", "OlusturanKullaniciId", "OlusturmaTarihi", "RolId", "YetkiTanimiId" },
                values: new object[,]
                {
                    { 1, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 10 },
                    { 3, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 11 },
                    { 4, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 12 },
                    { 5, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 13 },
                    { 6, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 20 },
                    { 7, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 21 },
                    { 8, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 22 },
                    { 9, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 23 },
                    { 10, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 30 },
                    { 11, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 31 },
                    { 12, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 32 },
                    { 13, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 33 },
                    { 14, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 40 },
                    { 15, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 41 },
                    { 16, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 42 },
                    { 17, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 43 },
                    { 18, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 50 },
                    { 19, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 51 },
                    { 20, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 52 },
                    { 21, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 60 },
                    { 22, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 61 },
                    { 23, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 70 },
                    { 24, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 71 },
                    { 25, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 72 },
                    { 26, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 73 },
                    { 27, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 80 },
                    { 28, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 81 },
                    { 29, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 30, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 10 },
                    { 31, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 11 },
                    { 32, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 12 },
                    { 33, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 20 },
                    { 34, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 21 },
                    { 35, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 22 },
                    { 36, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 30 },
                    { 37, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 31 },
                    { 38, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 32 },
                    { 39, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 40 },
                    { 40, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 41 },
                    { 41, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 42 },
                    { 42, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 50 },
                    { 43, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 51 },
                    { 44, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 52 },
                    { 45, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 },
                    { 46, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 10 },
                    { 47, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 20 },
                    { 48, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 30 },
                    { 49, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 40 },
                    { 50, true, null, null, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolYetkileri_RolId_YetkiTanimiId",
                table: "RolYetkileri",
                columns: new[] { "RolId", "YetkiTanimiId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolYetkileri_YetkiTanimiId",
                table: "RolYetkileri",
                column: "YetkiTanimiId");

            migrationBuilder.CreateIndex(
                name: "IX_YetkiTanimlari_YetkiKodu",
                table: "YetkiTanimlari",
                column: "YetkiKodu",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolYetkileri");

            migrationBuilder.DropTable(
                name: "YetkiTanimlari");
        }
    }
}
