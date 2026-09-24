using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hhuz.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionToAttributesAndProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Attributes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Attributes");
        }
    }
}
