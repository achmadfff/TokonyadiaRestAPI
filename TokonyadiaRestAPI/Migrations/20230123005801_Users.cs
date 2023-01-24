using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokonyadiaRestAPII.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "dbo",
                table: "customer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_customer_user_id",
                schema: "dbo",
                table: "customer",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_customer_user_user_id",
                schema: "dbo",
                table: "customer",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_user_user_id",
                schema: "dbo",
                table: "customer");

            migrationBuilder.DropIndex(
                name: "IX_customer_user_id",
                schema: "dbo",
                table: "customer");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "dbo",
                table: "customer");
        }
    }
}
