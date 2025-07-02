using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinancialTracker.Services.Analytics.DataAccess.Repositories
{
    /// <inheritdoc />
    public partial class AddIncomes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId_ExpenseId",
                table: "Expenses");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Expenses",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    IncomeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncomeTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.IncomeId);
                    table.ForeignKey(
                        name: "FK_Incomes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId_AccountId_ExpenseTime",
                table: "Expenses",
                columns: new[] { "UserId", "AccountId", "ExpenseTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId_ExpenseTime",
                table: "Expenses",
                columns: new[] { "UserId", "ExpenseTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserId_AccountId_IncomeTime",
                table: "Incomes",
                columns: new[] { "UserId", "AccountId", "IncomeTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserId_IncomeTime",
                table: "Incomes",
                columns: new[] { "UserId", "IncomeTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId_AccountId_ExpenseTime",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId_ExpenseTime",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Expenses");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId_ExpenseId",
                table: "Expenses",
                columns: new[] { "UserId", "ExpenseId" });
        }
    }
}
