namespace Microting.EformAngularFrontendBase.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixingPathForFolders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Link",
                value: "/advanced/folders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Link",
                value: "/folders");
        }
    }
}
