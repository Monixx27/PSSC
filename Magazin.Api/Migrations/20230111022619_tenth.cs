using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Magazin.Api.Migrations
{
    public partial class tenth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateLivrare",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipLivrare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AWB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Furnizor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sediu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cumparator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Scadenta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateLivrare", x => x.FacturaId);
                });

            migrationBuilder.CreateTable(
                name: "Facturi",
                columns: table => new
                {
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateLivrare");

            migrationBuilder.DropTable(
                name: "Facturi");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
