using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPrinters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterCartridges",
                columns: table => new
                {
                    CartridgesId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrintersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterCartridges", x => new { x.CartridgesId, x.PrintersId });
                    table.ForeignKey(
                        name: "FK_PrinterCartridges_Cartridges_CartridgesId",
                        column: x => x.CartridgesId,
                        principalTable: "Cartridges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrinterCartridges_Printers_PrintersId",
                        column: x => x.PrintersId,
                        principalTable: "Printers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrinterCartridges_PrintersId",
                table: "PrinterCartridges",
                column: "PrintersId");

            migrationBuilder.CreateIndex(
                name: "IX_Printers_Model",
                table: "Printers",
                column: "Model",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrinterCartridges");

            migrationBuilder.DropTable(
                name: "Printers");
        }
    }
}
