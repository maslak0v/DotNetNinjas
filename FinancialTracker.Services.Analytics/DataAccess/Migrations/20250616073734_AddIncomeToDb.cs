using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTracker.Services.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_User_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_User_UserId",
                table: "Incomes");
        }
    }
}
