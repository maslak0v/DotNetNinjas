using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWalletIdToAccountIdInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_WalletId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "WalletId",
                table: "Transactions",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                newName: "IX_Transactions_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Transactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_AccountId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Transactions",
                newName: "WalletId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                newName: "IX_Transactions_WalletId");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Transactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
