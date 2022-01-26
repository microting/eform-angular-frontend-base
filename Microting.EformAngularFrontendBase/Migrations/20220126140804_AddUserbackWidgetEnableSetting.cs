using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    public partial class AddUserbackWidgetEnableSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserbackEnabled",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserbackEnabled",
                table: "Users");
        }
    }
}
