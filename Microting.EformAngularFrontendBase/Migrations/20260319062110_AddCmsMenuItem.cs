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
            // Insert MenuTemplate 13 (CMS) if it doesn't already exist
            migrationBuilder.Sql(@"
                INSERT INTO MenuTemplates (Id, CreatedAt, CreatedByUserId, DefaultLink, E2EId, Name, UpdatedByUserId, Version)
                SELECT 13, '0001-01-01', 0, '/cms', 'cms', 'CMS', 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuTemplates WHERE Id = 13);
            ");

            // Insert MenuItem 13 (CMS) if it doesn't already exist
            migrationBuilder.Sql(@"
                INSERT INTO MenuItems (Id, CreatedAt, CreatedByUserId, E2EId, IsInternalLink, Link, MenuTemplateId, Name, Position, Type, UpdatedByUserId, Version)
                SELECT 13, '0001-01-01', 0, 'cms', 1, '/cms', 13, 'CMS', 9, 1, 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = 13);
            ");

            // Set ParentId = 3 (Advanced dropdown) only if that row exists
            migrationBuilder.Sql(@"
                UPDATE MenuItems SET ParentId = 3
                WHERE Id = 13
                AND EXISTS (SELECT 1 FROM (SELECT Id FROM MenuItems WHERE Id = 3) AS parent);
            ");

            // Insert MenuItemTranslations — no hardcoded IDs, dedup by (MenuItemId, LocaleName)
            migrationBuilder.Sql(@"
                INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
                SELECT '0001-01-01', 0, 'English', 'en-US', 13, 'CMS', 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'en-US');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
                SELECT '0001-01-01', 0, 'Danish', 'da', 13, 'CMS', 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'da');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
                SELECT '0001-01-01', 0, 'German', 'de-DE', 13, 'CMS', 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'de-DE');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
                SELECT '0001-01-01', 0, 'Ukrainian', 'uk-UA', 13, 'CMS', 0, 0
                WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'uk-UA');
            ");

            // Insert MenuTemplateTranslations — no hardcoded IDs, dedup by (MenuTemplateId, LocaleName)
            migrationBuilder.Sql(@"
                INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
                SELECT '0001-01-01', 'English', 'en-US', 13, 'CMS'
                WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'en-US');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
                SELECT '0001-01-01', 'Danish', 'da', 13, 'CMS'
                WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'da');
            ");

            migrationBuilder.Sql(@"
                INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
                SELECT '0001-01-01', 'German', 'de-DE', 13, 'CMS'
                WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'de-DE');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName IN ('en-US', 'da', 'de-DE');");

            migrationBuilder.Sql(
                "DELETE FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName IN ('en-US', 'da', 'de-DE', 'uk-UA');");

            migrationBuilder.Sql(
                "DELETE FROM MenuItems WHERE Id = 13;");

            migrationBuilder.Sql(
                "DELETE FROM MenuTemplates WHERE Id = 13;");
        }
    }
}
