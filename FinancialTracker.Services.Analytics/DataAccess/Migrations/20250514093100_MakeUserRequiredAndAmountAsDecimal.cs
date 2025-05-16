using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialTracker.Services.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserRequiredAndAmountAsDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Expenses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Expenses",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_User_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
