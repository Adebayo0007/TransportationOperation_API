using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class doneWithBudget2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Expenditures");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcurementApproved",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ItemQuantity",
                table: "Expenditures",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Expenditures",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcurementApproved",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Expenditures");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Expenditures",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
