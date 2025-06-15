using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseSinglePrimaryKeyInTransactionTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionTagId",
                table: "TransactionTags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags",
                column: "TransactionTagId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTags_TransactionId_TagId",
                table: "TransactionTags",
                columns: new[] { "TransactionId", "TagId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags");

            migrationBuilder.DropIndex(
                name: "IX_TransactionTags_TransactionId_TagId",
                table: "TransactionTags");

            migrationBuilder.DropColumn(
                name: "TransactionTagId",
                table: "TransactionTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags",
                columns: new[] { "TransactionId", "TagId" });
        }
    }
}
