using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Asee.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeCalculationHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", nullable: false),
                    InputJson = table.Column<string>(type: "TEXT", nullable: false),
                    OutputJson = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCalculationHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeRules",
                columns: table => new
                {
                    RuleId = table.Column<string>(type: "TEXT", nullable: false),
                    RuleType = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Criteria = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeRules", x => x.RuleId);
                });

            migrationBuilder.InsertData(
                table: "FeeRules",
                columns: new[] { "RuleId", "Action", "Amount", "Criteria", "Description", "IsActive", "RuleType" },
                values: new object[,]
                {
                    { "1", "fixed_fee", 0.2m, "{\"maxAmount\": 100}", "Fixed fee for POS transactions", true, "POS" },
                    { "2", "percentage_fee", 0.18m, "{\"minAmount\": 10}", "Percentage fee for e-commerce transactions", true, "E-commerce" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeCalculationHistory");

            migrationBuilder.DropTable(
                name: "FeeRules");
        }
    }
}
