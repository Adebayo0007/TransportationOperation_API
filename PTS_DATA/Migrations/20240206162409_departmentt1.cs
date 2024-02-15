using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTS_DATA.Migrations
{
    public partial class departmentt1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepartmentalExpenditureBudgets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    ModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentalExpenditureBudgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentalSales",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsModified = table.Column<bool>(type: "bit", nullable: true),
                    ModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifierId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentalSales", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentalExpenditureBudgets");

            migrationBuilder.DropTable(
                name: "DepartmentalSales");
        }
    }
}
