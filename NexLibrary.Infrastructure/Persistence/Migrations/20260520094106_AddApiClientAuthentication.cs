using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddApiClientAuthentication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiClients",
                columns: table => new
                {
                    ApiClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ClientSecretHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ClientSecretSalt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SonKullanimTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClients", x => x.ApiClientId);
                });

            migrationBuilder.CreateTable(
                name: "ApiClientYetkileri",
                columns: table => new
                {
                    ApiClientYetkiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiClientId = table.Column<int>(type: "int", nullable: false),
                    YetkiTanimiId = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClientYetkileri", x => x.ApiClientYetkiId);
                    table.ForeignKey(
                        name: "FK_ApiClientYetkileri_ApiClients_ApiClientId",
                        column: x => x.ApiClientId,
                        principalTable: "ApiClients",
                        principalColumn: "ApiClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiClientYetkileri_YetkiTanimlari_YetkiTanimiId",
                        column: x => x.YetkiTanimiId,
                        principalTable: "YetkiTanimlari",
                        principalColumn: "YetkiTanimiId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiClients_ClientId",
                table: "ApiClients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiClientYetkileri_ApiClientId_YetkiTanimiId",
                table: "ApiClientYetkileri",
                columns: new[] { "ApiClientId", "YetkiTanimiId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiClientYetkileri_YetkiTanimiId",
                table: "ApiClientYetkileri",
                column: "YetkiTanimiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiClientYetkileri");

            migrationBuilder.DropTable(
                name: "ApiClients");
        }
    }
}
