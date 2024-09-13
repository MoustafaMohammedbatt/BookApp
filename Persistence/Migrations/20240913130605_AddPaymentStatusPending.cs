using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusPending : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "UserCarts");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "paymentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "paymentForms");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "UserCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
