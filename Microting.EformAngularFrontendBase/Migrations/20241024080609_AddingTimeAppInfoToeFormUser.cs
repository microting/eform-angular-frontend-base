using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microting.EformAngularFrontendBase.Migrations
{
    /// <inheritdoc />
    public partial class AddingTimeAppInfoToeFormUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationLastIp",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationLastKnownLocation",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationLookedUpIp",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationManufacturer",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationModel",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationOsVersion",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TimeRegistrationSoftwareVersion",
                table: "Users",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeRegistrationLastIp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationLastKnownLocation",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationLookedUpIp",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationManufacturer",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationModel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationOsVersion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeRegistrationSoftwareVersion",
                table: "Users");
        }
    }
}
