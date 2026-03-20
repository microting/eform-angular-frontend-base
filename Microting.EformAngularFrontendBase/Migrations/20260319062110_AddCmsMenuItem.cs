using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Microting.EformAngularFrontendBase.Migrations
{
    /// <inheritdoc />
    public partial class AddCmsMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuTemplates",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "DefaultLink", "E2EId", "EformPluginId", "Name", "UpdatedAt", "UpdatedByUserId", "Version", "WorkflowState" },
                values: new object[] { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "/cms", "cms", null, "CMS", null, 0, 0, null });

            // Insert with ParentId = NULL to avoid FK constraint issues if the parent row has been customised away
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "E2EId", "IconName", "IsInternalLink", "Link", "MenuTemplateId", "Name", "ParentId", "Position", "Type", "UpdatedAt", "UpdatedByUserId", "Version", "WorkflowState" },
                values: new object[] { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "cms", null, true, "/cms", 13, "CMS", null, 9, 1, null, 0, 0, null });

            // Now set ParentId = 3 (the "Advanced" dropdown) only if that row still exists
            migrationBuilder.Sql(
                "UPDATE MenuItems SET ParentId = 3 WHERE Id = 13 AND EXISTS (SELECT 1 FROM (SELECT Id FROM MenuItems WHERE Id = 3) AS parent);");

            migrationBuilder.InsertData(
                table: "MenuItemTranslations",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "Language", "LocaleName", "MenuItemId", "Name", "UpdatedAt", "UpdatedByUserId", "Version", "WorkflowState" },
                values: new object[,]
                {
                    { 51, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "English", "en-US", 13, "CMS", null, 0, 0, null },
                    { 52, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Danish", "da", 13, "CMS", null, 0, 0, null },
                    { 53, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "German", "de-DE", 13, "CMS", null, 0, 0, null },
                    { 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Ukrainian", "uk-UA", 13, "CMS", null, 0, 0, null }
                });

            migrationBuilder.InsertData(
                table: "MenuTemplateTranslations",
                columns: new[] { "Id", "CreatedAt", "Language", "LocaleName", "MenuTemplateId", "Name", "UpdatedAt", "Version", "WorkflowState" },
                values: new object[,]
                {
                    { 37, null, "English", "en-US", 13, "CMS", null, null, null },
                    { 38, null, "Danish", "da", 13, "CMS", null, null, null },
                    { 39, null, "German", "de-DE", 13, "CMS", null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuTemplateTranslations",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "MenuTemplateTranslations",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "MenuTemplateTranslations",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "MenuItemTranslations",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "MenuItemTranslations",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "MenuItemTranslations",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "MenuItemTranslations",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuTemplates",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
