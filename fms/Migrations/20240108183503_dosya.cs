using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fms.Migrations
{
    /// <inheritdoc />
    public partial class dosya : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dosyalar",
                columns: table => new
                {
                    DosyaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DosyaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosyaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DosyaPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DosyaTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosyalar", x => x.DosyaId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dosyalar");
        }
    }
}
