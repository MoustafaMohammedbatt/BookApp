using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookLanguage",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "BookLanguage",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
