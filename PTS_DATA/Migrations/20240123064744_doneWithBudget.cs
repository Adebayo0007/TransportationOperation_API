using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class doneWithBudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditorComment",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DDPComment",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAuditorCommented",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChairmanApproved",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDDPCommented",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsResolved",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Expenditures",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestType",
                table: "Expenditures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StoreItemId",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreItemName",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerminalId",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerminalName",
                table: "Expenditures",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditorComment",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "DDPComment",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "IsAuditorCommented",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "IsChairmanApproved",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "IsDDPCommented",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "IsResolved",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "StoreItemId",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "StoreItemName",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "TerminalId",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "TerminalName",
                table: "Expenditures");
        }
    }
}
