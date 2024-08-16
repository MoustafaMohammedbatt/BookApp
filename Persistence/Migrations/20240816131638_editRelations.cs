using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Renteds_AspNetUsers_ReceptionId",
                table: "Renteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Solds_AspNetUsers_ReceptionId",
                table: "Solds");

            migrationBuilder.DropIndex(
                name: "IX_Solds_ReceptionId",
                table: "Solds");

            migrationBuilder.DropIndex(
                name: "IX_Renteds_ReceptionId",
                table: "Renteds");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ReceptionId",
                table: "Solds");

            migrationBuilder.DropColumn(
                name: "ReceptionId",
                table: "Renteds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceptionId",
                table: "Solds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceptionId",
                table: "Renteds",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solds_ReceptionId",
                table: "Solds",
                column: "ReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Renteds_ReceptionId",
                table: "Renteds",
                column: "ReceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Renteds_AspNetUsers_ReceptionId",
                table: "Renteds",
                column: "ReceptionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_AspNetUsers_ReceptionId",
                table: "Solds",
                column: "ReceptionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
