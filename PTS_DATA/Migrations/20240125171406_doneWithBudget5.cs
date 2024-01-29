using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class doneWithBudget5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusBrandingRequests");

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "BusBrandings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PartnerName",
                table: "BusBrandings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "BusBrandings");

            migrationBuilder.DropColumn(
                name: "PartnerName",
                table: "BusBrandings");

            migrationBuilder.CreateTable(
                name: "BusBrandingRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AuditorComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailabilityType = table.Column<int>(type: "int", nullable: true),
                    BrandEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrandStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DDPComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAuditorCommented = table.Column<bool>(type: "bit", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true),
                    IsChairmanApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsDDPCommented = table.Column<bool>(type: "bit", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    NumberOfVehicle = table.Column<long>(type: "bigint", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    Reciept = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusBrandingRequests", x => x.Id);
                });
        }
    }
}
