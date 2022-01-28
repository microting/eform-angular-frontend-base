using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    public partial class AddUserbackTokenToSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConfigurationValues",
                columns: new[] { "Id", "Value" },
                values: new object[] { "ApplicationSettings:UserbackToken", "33542|62605|dEaGb7GN0RoGEOMwEEWGh1pnh" });

            migrationBuilder.InsertData(
                table: "ConfigurationValues",
                columns: new[] { "Id", "Value" },
                values: new object[] { "ApplicationSettings:IsUserbackWidgetEnabled", "false" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationValues",
                keyColumn: "Id",
                keyValue: "ApplicationSettings:UserbackToken");

            migrationBuilder.InsertData(
                table: "ConfigurationValues",
                columns: new[] { "Id", "Value" },
                values: new object[] { "ApplicationSettings:IsUserbackWidgetEnabled", "true" });
        }
    }
}
