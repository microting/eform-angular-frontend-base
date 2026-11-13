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
            // Do not remove defaultValue: this is the first string DDL default in this repo's 52
            // migrations, and the next migration (BackfillThemeModeFromDarkTheme) depends on it
            // to mark existing rows with 'system'. Without it they go NULL and the backfill no-ops.
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
