using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    public partial class AddUserbackWidgetEnableSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConfigurationValues",
                columns: new[] { "Id", "Value" },
                values: new object[] { "ApplicationSettings:IsUserbackWidgetEnabled", "true" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationValues",
                keyColumn: "Id",
                keyValue: "ApplicationSettings:IsUserbackWidgetEnabled");
        }
    }
}
