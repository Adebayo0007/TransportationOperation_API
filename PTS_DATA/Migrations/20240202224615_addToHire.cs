using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class addToHire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Kilometer",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kilometer",
                table: "HireVehicles");
        }
    }
}
