using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Authors");
        }
    }
}
