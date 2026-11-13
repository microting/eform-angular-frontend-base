using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    /// <inheritdoc />
    public partial class BackfillThemeModeFromDarkTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Every row that exists now was created under the DarkTheme checkbox, so it
            // carries a deliberate choice. Preserve it: the column default would silently
            // move the entire user base onto "follow system".
            // Kept separate from AddThemeModeToUser so a failure here cannot leave the
            // column added but unrecorded (MariaDB implicitly commits DDL), which would
            // crash-loop the next startup on "Duplicate column name". The WHERE clause
            // makes a retry idempotent: only rows still on the column default are touched,
            // so a preference chosen since cannot be overwritten.
            migrationBuilder.Sql(
                "UPDATE Users SET ThemeMode = IF(DarkTheme = 1, 'dark', 'light') WHERE ThemeMode = 'system';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty: the original per-user values are not recoverable,
            // and AddThemeModeToUser.Down drops the column anyway.
        }
    }
}
