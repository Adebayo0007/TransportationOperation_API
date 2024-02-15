using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class departmentt11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "DepartmentalSales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "DepartmentalExpenditureBudgets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "DepartmentalSales");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "DepartmentalExpenditureBudgets");
        }
    }
}
