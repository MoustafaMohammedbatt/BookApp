using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addUserCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Renteds_Carts_CartId",
                table: "Renteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Solds_Carts_CartId",
                table: "Solds");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "Solds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserCartId",
                table: "Solds",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "Renteds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserCartId",
                table: "Renteds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserCart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solds_UserCartId",
                table: "Solds",
                column: "UserCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Renteds_UserCartId",
                table: "Renteds",
                column: "UserCartId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCart_UserId",
                table: "UserCart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Renteds_Carts_CartId",
                table: "Renteds",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Renteds_UserCart_UserCartId",
                table: "Renteds",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_Carts_CartId",
                table: "Solds",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_UserCart_UserCartId",
                table: "Solds",
                column: "UserCartId",
                principalTable: "UserCart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Renteds_Carts_CartId",
                table: "Renteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Renteds_UserCart_UserCartId",
                table: "Renteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Solds_Carts_CartId",
                table: "Solds");

            migrationBuilder.DropForeignKey(
                name: "FK_Solds_UserCart_UserCartId",
                table: "Solds");

            migrationBuilder.DropTable(
                name: "UserCart");

            migrationBuilder.DropIndex(
                name: "IX_Solds_UserCartId",
                table: "Solds");

            migrationBuilder.DropIndex(
                name: "IX_Renteds_UserCartId",
                table: "Renteds");

            migrationBuilder.DropColumn(
                name: "UserCartId",
                table: "Solds");

            migrationBuilder.DropColumn(
                name: "UserCartId",
                table: "Renteds");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "Solds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "Renteds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Renteds_Carts_CartId",
                table: "Renteds",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solds_Carts_CartId",
                table: "Solds",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
