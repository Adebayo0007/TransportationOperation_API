using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class now1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HireVehicleRequests_HireVehicles_HireVehicleId",
                table: "HireVehicleRequests");

            migrationBuilder.DropIndex(
                name: "IX_HireVehicleRequests_HireVehicleId",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "HireVehicleId",
                table: "HireVehicleRequests");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "HireVehicleRequests",
                newName: "CostOfExacution");

            migrationBuilder.AddColumn<double>(
                name: "CostOfExacution",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Fuel",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Maintenance",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Profit",
                table: "HireVehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "RecieptImage",
                table: "HireVehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "HireVehicleRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "HireVehicleRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeapartureDate",
                table: "HireVehicleRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DepartureAddress",
                table: "HireVehicleRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartureTerminalId",
                table: "HireVehicleRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestinationAddress",
                table: "HireVehicleRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverUserId",
                table: "HireVehicleRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleId",
                table: "HireVehicleRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostOfExacution",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "Fuel",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "Maintenance",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "Profit",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "RecieptImage",
                table: "HireVehicles");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "DeapartureDate",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "DepartureAddress",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "DepartureTerminalId",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "DestinationAddress",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "DriverUserId",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "HireVehicleRequests");

            migrationBuilder.RenameColumn(
                name: "CostOfExacution",
                table: "HireVehicleRequests",
                newName: "Cost");

            migrationBuilder.AddColumn<string>(
                name: "HireVehicleId",
                table: "HireVehicleRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HireVehicleRequests_HireVehicleId",
                table: "HireVehicleRequests",
                column: "HireVehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_HireVehicleRequests_HireVehicles_HireVehicleId",
                table: "HireVehicleRequests",
                column: "HireVehicleId",
                principalTable: "HireVehicles",
                principalColumn: "Id");
        }
    }
}
