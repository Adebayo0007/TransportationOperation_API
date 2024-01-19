using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailabilityType",
                table: "StoreItemRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityType",
                table: "StoreAssetRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityType",
                table: "OtherRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "OtherRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "OtherRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "OtherRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OtherRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityType",
                table: "HireVehicleRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityType",
                table: "BusBrandingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityType",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "AvailabilityType",
                table: "StoreAssetRequests");

            migrationBuilder.DropColumn(
                name: "AvailabilityType",
                table: "OtherRequests");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OtherRequests");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "OtherRequests");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "OtherRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OtherRequests");

            migrationBuilder.DropColumn(
                name: "AvailabilityType",
                table: "HireVehicleRequests");

            migrationBuilder.DropColumn(
                name: "AvailabilityType",
                table: "BusBrandingRequests");
        }
    }
}
