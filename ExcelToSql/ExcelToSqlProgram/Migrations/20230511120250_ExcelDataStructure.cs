using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelToSqlProgram.Migrations
{
    /// <inheritdoc />
    public partial class ExcelDataStructure : Migration
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
                    DiscountBand = table.Column<int>(type: "int", nullable: true),
                    Units_Sold = table.Column<double>(type: "float", nullable: true),
                    Manufacturing_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sale_Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gross_Sales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discounts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sales = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COGS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
