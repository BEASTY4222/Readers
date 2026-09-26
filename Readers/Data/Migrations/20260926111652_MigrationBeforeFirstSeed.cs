using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readers.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationBeforeFirstSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pages",
                table: "Books",
                newName: "Pages");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Authors",
                newName: "Country");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImagePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pages",
                table: "Books",
                newName: "pages");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Authors",
                newName: "country");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImagePath",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
