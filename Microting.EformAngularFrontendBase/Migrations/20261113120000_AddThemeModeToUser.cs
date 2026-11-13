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

            // Every row that exists now was created under the DarkTheme checkbox, so it
            // carries a deliberate choice. Preserve it: the default above would silently
            // move the entire user base onto "follow system".
            migrationBuilder.Sql(
                "UPDATE Users SET ThemeMode = IF(DarkTheme = 1, 'dark', 'light');");
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
