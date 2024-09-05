using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "UserCarts");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "paymentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "paymentForms");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "UserCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
