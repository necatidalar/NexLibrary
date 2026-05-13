using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBookCopiesStockModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KitapKopyaId",
                table: "Oduncler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KitapKopyalari",
                columns: table => new
                {
                    KitapKopyaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitapId = table.Column<int>(type: "int", nullable: false),
                    Barkod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DemirbasNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Durum = table.Column<int>(type: "int", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    GuncelleyenKullaniciId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KitapKopyalari", x => x.KitapKopyaId);
                    table.ForeignKey(
                        name: "FK_KitapKopyalari_Kitaplar_KitapId",
                        column: x => x.KitapId,
                        principalTable: "Kitaplar",
                        principalColumn: "KitapId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oduncler_KitapKopyaId",
                table: "Oduncler",
                column: "KitapKopyaId");

            migrationBuilder.CreateIndex(
                name: "IX_KitapKopyalari_Barkod",
                table: "KitapKopyalari",
                column: "Barkod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KitapKopyalari_Durum",
                table: "KitapKopyalari",
                column: "Durum");

            migrationBuilder.CreateIndex(
                name: "IX_KitapKopyalari_KitapId",
                table: "KitapKopyalari",
                column: "KitapId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oduncler_KitapKopyalari_KitapKopyaId",
                table: "Oduncler",
                column: "KitapKopyaId",
                principalTable: "KitapKopyalari",
                principalColumn: "KitapKopyaId",
                onDelete: ReferentialAction.Restrict);


            migrationBuilder.Sql("""
INSERT INTO KitapKopyalari
(
    KitapId,
    Barkod,
    DemirbasNo,
    Durum,
    Aciklama,
    AktifMi,
    OlusturmaTarihi,
    GuncellemeTarihi,
    OlusturanKullaniciId,
    GuncelleyenKullaniciId
)
SELECT 
    k.KitapId,
    CONCAT('BK-', k.KitapId, '-001'),
    NULL,
    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM Oduncler o
            WHERE o.KitapId = k.KitapId
              AND o.AktifMi = 1
              AND o.Durum = 3
        ) THEN 3
        WHEN EXISTS
        (
            SELECT 1
            FROM Oduncler o
            WHERE o.KitapId = k.KitapId
              AND o.AktifMi = 1
              AND o.Durum = 1
        ) THEN 2
        ELSE 1
    END,
    N'Migration ile oluşturulan ilk kopya',
    CAST(1 AS bit),
    SYSUTCDATETIME(),
    NULL,
    NULL,
    NULL
FROM Kitaplar k
WHERE k.AktifMi = 1
AND NOT EXISTS
(
    SELECT 1
    FROM KitapKopyalari kk
    WHERE kk.KitapId = k.KitapId
);

UPDATE o
SET o.KitapKopyaId = kk.KitapKopyaId
FROM Oduncler o
INNER JOIN KitapKopyalari kk 
    ON kk.KitapId = o.KitapId
WHERE o.KitapKopyaId IS NULL
  AND o.AktifMi = 1
  AND o.Durum IN (1, 3);
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oduncler_KitapKopyalari_KitapKopyaId",
                table: "Oduncler");

            migrationBuilder.DropTable(
                name: "KitapKopyalari");

            migrationBuilder.DropIndex(
                name: "IX_Oduncler_KitapKopyaId",
                table: "Oduncler");

            migrationBuilder.DropColumn(
                name: "KitapKopyaId",
                table: "Oduncler");
        }
    }
}
