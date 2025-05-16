using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTracker.Services.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountCurrencyToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Expenses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Expenses",
                type: "text",
                nullable: false,
                defaultValue: "RUB");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Expenses");
        }
    }
}
