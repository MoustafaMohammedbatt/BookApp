using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateUserCartModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Renteds_UserCart_UserCartId",
                table: "Renteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Solds_UserCart_UserCartId",
                table: "Solds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCart_AspNetUsers_UserId",
                table: "UserCart");

            migrationBuilder.DropIndex(
                name: "IX_Renteds_UserCartId",
                table: "Renteds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart");

            migrationBuilder.DropColumn(
                name: "UserCartId",
                table: "Renteds");

            migrationBuilder.RenameTable(
                name: "UserCart",
                newName: "UserCarts");

            migrationBuilder.RenameIndex(
                name: "IX_UserCart_UserId",
                table: "UserCarts",
                newName: "IX_UserCarts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_UserCarts_UserCartId",
                table: "Solds",
                column: "UserCartId",
                principalTable: "UserCarts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCarts_AspNetUsers_UserId",
                table: "UserCarts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solds_UserCarts_UserCartId",
                table: "Solds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCarts_AspNetUsers_UserId",
                table: "UserCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCarts",
                table: "UserCarts");

            migrationBuilder.RenameTable(
                name: "UserCarts",
                newName: "UserCart");

            migrationBuilder.RenameIndex(
                name: "IX_UserCarts_UserId",
                table: "UserCart",
                newName: "IX_UserCart_UserId");

            migrationBuilder.AddColumn<int>(
                name: "UserCartId",
                table: "Renteds",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCart",
                table: "UserCart",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Renteds_UserCartId",
                table: "Renteds",
                column: "UserCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Renteds_UserCart_UserCartId",
                table: "Renteds",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_UserCart_UserCartId",
                table: "Solds",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCart_AspNetUsers_UserId",
                table: "UserCart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
