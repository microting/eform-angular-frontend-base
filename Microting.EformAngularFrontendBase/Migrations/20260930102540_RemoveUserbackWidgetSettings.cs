using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microting.EformAngularFrontendBase.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserbackWidgetSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationValues",
                keyColumn: "Id",
                keyValue: "ApplicationSettings:IsUserbackWidgetEnabled");

            migrationBuilder.DeleteData(
                table: "ConfigurationValues",
                keyColumn: "Id",
                keyValue: "ApplicationSettings:UserbackToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The token is restored empty on purpose: the Userback widget is gone, so the
            // key comes back for schema parity with older code, not the old credential.
            migrationBuilder.InsertData(
                table: "ConfigurationValues",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { "ApplicationSettings:IsUserbackWidgetEnabled", "false" },
                    { "ApplicationSettings:UserbackToken", "" }
                });
        }
    }
}
