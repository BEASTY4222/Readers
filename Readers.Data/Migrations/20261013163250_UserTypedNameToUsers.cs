using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readers.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserTypedNameToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserTypedName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserTypedName",
                table: "AspNetUsers");
        }
    }
}
