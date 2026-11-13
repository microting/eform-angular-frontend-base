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
            // crash-loop the next startup on "Duplicate column name".
            // The WHERE clause cannot tell a never-backfilled row from a user who has
            // deliberately chosen Follow system, so a re-run after a lost history row would
            // rewrite such users to light or dark. It is safe as deployed because migrations
            // run at startup before the pod serves traffic, so nobody can have chosen
            // "system" in the window between the two migrations.
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
