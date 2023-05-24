using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelToSqlProgram.Migrations
{
    /// <inheritdoc />
    public partial class ExcelData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount_Band = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Units_Sold = table.Column<float>(type: "real", nullable: true),
                    Manufacturing_Price = table.Column<float>(type: "real", nullable: true),
                    Sale_Price = table.Column<float>(type: "real", nullable: true),
                    Gross_Sales = table.Column<float>(type: "real", nullable: true),
                    Discounts = table.Column<float>(type: "real", nullable: true),
                    Sales = table.Column<float>(type: "real", nullable: true),
                    COGS = table.Column<float>(type: "real", nullable: true),
                    Profit = table.Column<float>(type: "real", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelData");
        }
    }
}
