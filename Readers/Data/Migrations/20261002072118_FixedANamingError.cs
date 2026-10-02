using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readers.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedANamingError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ganre",
                table: "Books",
                newName: "Genre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "Ganre");
        }
    }
}
