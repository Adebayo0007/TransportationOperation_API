using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class nnextdon1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "Maintenance",
                table: "HireVehicles");

            migrationBuilder.RenameColumn(
                name: "RecieptImage",
                table: "HireVehicles",
                newName: "RecieptAndInvoice");

            migrationBuilder.AddColumn<bool>(
                name: "IsChairmanApprove",
                table: "HireVehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "HireVehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ResolvedByDepo",
                table: "HireVehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ResolvedByOperation",
                table: "HireVehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChairmanApprove",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "ResolvedByDepo",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "ResolvedByOperation",
                table: "HireVehicles");

            migrationBuilder.RenameColumn(
                name: "RecieptAndInvoice",
                table: "HireVehicles",
                newName: "RecieptImage");

            migrationBuilder.AddColumn<double>(
                name: "Maintenance",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "HireVehicleRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditorComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailabilityType = table.Column<int>(type: "int", nullable: true),
                    CostOfExacution = table.Column<double>(type: "float", nullable: false),
                    DDPComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeapartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureTerminalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DriverUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fuel = table.Column<double>(type: "float", nullable: false),
                    IsAuditorCommented = table.Column<bool>(type: "bit", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true),
                    IsChairmanApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsDDPCommented = table.Column<bool>(type: "bit", nullable: true),
                    IsResolved = table.Column<bool>(type: "bit", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true),
                    Maintenance = table.Column<double>(type: "float", nullable: false),
                    Profit = table.Column<double>(type: "float", nullable: false),
                    RecieptImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestType = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HireVehicleRequests", x => x.Id);
                });
        }
    }
}
