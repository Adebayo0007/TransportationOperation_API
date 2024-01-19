using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class now : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusBrandingRequests_BusBrandings_BusBrandingId",
                table: "BusBrandingRequests");

            migrationBuilder.DropIndex(
                name: "IX_BusBrandingRequests_BusBrandingId",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "BusBrandingId",
                table: "BusBrandingRequests");

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "BusBrandings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "BusBrandingRequests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BrandEndDate",
                table: "BusBrandingRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BrandStartDate",
                table: "BusBrandingRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BusBrandingRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "NumberOfVehicle",
                table: "BusBrandingRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "BusBrandingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reciept",
                table: "BusBrandingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "BusBrandingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "BusBrandings");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "BrandEndDate",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "BrandStartDate",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "NumberOfVehicle",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "Reciept",
                table: "BusBrandingRequests");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "BusBrandingRequests");

            migrationBuilder.AddColumn<string>(
                name: "BusBrandingId",
                table: "BusBrandingRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusBrandingRequests_BusBrandingId",
                table: "BusBrandingRequests",
                column: "BusBrandingId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusBrandingRequests_BusBrandings_BusBrandingId",
                table: "BusBrandingRequests",
                column: "BusBrandingId",
                principalTable: "BusBrandings",
                principalColumn: "Id");
        }
    }
}
