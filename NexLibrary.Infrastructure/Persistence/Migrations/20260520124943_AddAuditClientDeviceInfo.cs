using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexLibrary.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditClientDeviceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CihazBilgisi",
                table: "AuditLoglari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Dil",
                table: "AuditLoglari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeaderJson",
                table: "AuditLoglari",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "AuditLoglari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HttpMethod",
                table: "AuditLoglari",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IsletimSistemi",
                table: "AuditLoglari",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MacAdresi",
                table: "AuditLoglari",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referer",
                table: "AuditLoglari",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestPath",
                table: "AuditLoglari",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TarayiciBilgisi",
                table: "AuditLoglari",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "AuditLoglari",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_IpAdresi",
                table: "AuditLoglari",
                column: "IpAdresi");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLoglari_IslemTuru",
                table: "AuditLoglari",
                column: "IslemTuru");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditLoglari_IpAdresi",
                table: "AuditLoglari");

            migrationBuilder.DropIndex(
                name: "IX_AuditLoglari_IslemTuru",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "CihazBilgisi",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "Dil",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "HeaderJson",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "Host",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "HttpMethod",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "IsletimSistemi",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "MacAdresi",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "Referer",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "RequestPath",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "TarayiciBilgisi",
                table: "AuditLoglari");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "AuditLoglari");
        }
    }
}
