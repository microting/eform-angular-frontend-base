using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    /// <inheritdoc />
    public partial class AddThemeModeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThemeMode",
                table: "Users",
                type: "longtext",
                nullable: true,
                defaultValue: "system")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThemeMode",
                table: "Users");
        }
    }
}
