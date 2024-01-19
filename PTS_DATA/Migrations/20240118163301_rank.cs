using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class rank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreAssetSignature",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "Signature",
                table: "StoreAssets");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "StoreItemRequests",
                newName: "StoreAssetId");

            migrationBuilder.RenameColumn(
                name: "TerminalId",
                table: "StoreItemRequests",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "IsTechnical",
                table: "StoreItemRequests",
                newName: "IsModified");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "StoreItemRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "StoreItemRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "StoreItemRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StoreItemRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "StoreItemRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierId",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifierName",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreItemType",
                table: "StoreItemRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRegistrationNumber",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "StoreAssetRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "OtherRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "HireVehicleRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "BusBrandingRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "ModifierId",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "ModifierName",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "StoreItemType",
                table: "StoreItemRequests");

            migrationBuilder.DropColumn(
                name: "VehicleRegistrationNumber",
                table: "StoreItemRequests");

            migrationBuilder.RenameColumn(
                name: "StoreAssetId",
                table: "StoreItemRequests",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "IsModified",
                table: "StoreItemRequests",
                newName: "IsTechnical");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "StoreItemRequests",
                newName: "TerminalId");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "StoreItemRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreAssetSignature",
                table: "StoreItemRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "StoreAssets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "StoreAssetRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "OtherRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "HireVehicleRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AvailabilityType",
                table: "BusBrandingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
