/*
The MIT License (MIT)

Copyright (c) 2007 - 2026 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CmsMigrationIdempotencyTests : DbTestFixture
{
    private const string IdempotentCmsSeedSql = @"
        INSERT INTO MenuTemplates (Id, CreatedAt, CreatedByUserId, DefaultLink, E2EId, Name, UpdatedByUserId, Version)
        SELECT 13, '0001-01-01', 0, '/cms', 'cms', 'CMS', 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuTemplates WHERE Id = 13);

        INSERT INTO MenuItems (Id, CreatedAt, CreatedByUserId, E2EId, IsInternalLink, Link, MenuTemplateId, Name, Position, Type, UpdatedByUserId, Version)
        SELECT 13, '0001-01-01', 0, 'cms', 1, '/cms', 13, 'CMS', 9, 1, 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuItems WHERE Id = 13);

        UPDATE MenuItems SET ParentId = 3 WHERE Id = 13 AND EXISTS (SELECT 1 FROM (SELECT Id FROM MenuItems WHERE Id = 3) AS parent);

        INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
        SELECT '0001-01-01', 0, 'English', 'en-US', 13, 'CMS', 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'en-US');

        INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
        SELECT '0001-01-01', 0, 'Danish', 'da', 13, 'CMS', 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'da');

        INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
        SELECT '0001-01-01', 0, 'German', 'de-DE', 13, 'CMS', 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'de-DE');

        INSERT INTO MenuItemTranslations (CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
        SELECT '0001-01-01', 0, 'Ukrainian', 'uk-UA', 13, 'CMS', 0, 0
        WHERE NOT EXISTS (SELECT 1 FROM MenuItemTranslations WHERE MenuItemId = 13 AND LocaleName = 'uk-UA');

        INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
        SELECT '0001-01-01', 'English', 'en-US', 13, 'CMS'
        WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'en-US');

        INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
        SELECT '0001-01-01', 'Danish', 'da', 13, 'CMS'
        WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'da');

        INSERT INTO MenuTemplateTranslations (CreatedAt, Language, LocaleName, MenuTemplateId, Name)
        SELECT '0001-01-01', 'German', 'de-DE', 13, 'CMS'
        WHERE NOT EXISTS (SELECT 1 FROM MenuTemplateTranslations WHERE MenuTemplateId = 13 AND LocaleName = 'de-DE');
    ";

    [Test]
    public async Task Migration_CleanDatabase_CreatesCmsMenuItemWithTranslations()
    {
        // Arrange — migration already ran in DbTestFixture.Setup() via Database.Migrate()

        // Act
        var menuItem = await DbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.E2EId == "cms");

        var menuTemplate = await DbContext.MenuTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(mt => mt.E2EId == "cms");

        // Assert — CMS menu item exists
        Assert.That(menuItem, Is.Not.Null, "CMS MenuItem should exist after migration");
        Assert.That(menuItem.Name, Is.EqualTo("CMS"));
        Assert.That(menuItem.Link, Is.EqualTo("/cms"));
        Assert.That(menuItem.MenuTemplateId, Is.EqualTo(menuTemplate.Id));

        // Assert — CMS menu template exists
        Assert.That(menuTemplate, Is.Not.Null, "CMS MenuTemplate should exist after migration");
        Assert.That(menuTemplate.Name, Is.EqualTo("CMS"));

        // Assert — translations exist for all 4 languages
        var itemTranslations = await DbContext.MenuItemTranslations
            .AsNoTracking()
            .Where(t => t.MenuItemId == menuItem.Id)
            .ToListAsync();

        Assert.That(itemTranslations.Count, Is.EqualTo(4));
        Assert.That(itemTranslations.Select(t => t.LocaleName),
            Is.EquivalentTo(new[] { "en-US", "da", "de-DE", "uk-UA" }));

        // Assert — template translations exist for all 3 languages
        var templateTranslations = await DbContext.MenuTemplateTranslations
            .AsNoTracking()
            .Where(t => t.MenuTemplateId == menuTemplate.Id)
            .ToListAsync();

        Assert.That(templateTranslations.Count, Is.EqualTo(3));
        Assert.That(templateTranslations.Select(t => t.LocaleName),
            Is.EquivalentTo(new[] { "en-US", "da", "de-DE" }));
    }

    [Test]
    public async Task Migration_ConflictingTranslationIds_StillCreatesCmsTranslations()
    {
        // Arrange — seed initial CMS data (simulating the initial clean migration run)
        await DbContext.Database.ExecuteSqlRawAsync(IdempotentCmsSeedSql);

        var cmsMenuItem = await DbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.E2EId == "cms");
        Assert.That(cmsMenuItem, Is.Not.Null, "CMS MenuItem should exist after initial seed");

        // Delete the CMS translations so we can test re-insertion
        await DbContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM MenuItemTranslations WHERE MenuItemId = (SELECT Id FROM MenuItems WHERE E2EId = 'cms')");
        await DbContext.Database.ExecuteSqlRawAsync(
            "DELETE FROM MenuTemplateTranslations WHERE MenuTemplateId = (SELECT Id FROM MenuTemplates WHERE E2EId = 'cms')");

        // Insert a dummy MenuTemplate and MenuItem so foreign keys are satisfied for conflicting rows
        await DbContext.Database.ExecuteSqlRawAsync(@"
        INSERT INTO MenuTemplates (Id, CreatedAt, CreatedByUserId, DefaultLink, E2EId, Name, UpdatedByUserId, Version)
        VALUES (1, '2020-01-01', 0, '/fake', 'fake', 'FakePlugin', 0, 0);

        INSERT INTO MenuItems (Id, CreatedAt, CreatedByUserId, E2EId, IsInternalLink, Link, MenuTemplateId, Name, Position, Type, UpdatedByUserId, Version)
        VALUES (1, '2020-01-01', 0, 'fake', 1, '/fake', 1, 'FakePlugin', 1, 1, 0, 0);");

        // Insert conflicting rows at IDs 51-54 for a DIFFERENT menu item (Id=1)
        await DbContext.Database.ExecuteSqlRawAsync(@"
        INSERT INTO MenuItemTranslations (Id, CreatedAt, CreatedByUserId, Language, LocaleName, MenuItemId, Name, UpdatedByUserId, Version)
        VALUES (51, '2020-01-01', 0, 'English', 'en-US', 1, 'FakePlugin', 0, 0),
               (52, '2020-01-01', 0, 'Danish', 'da', 1, 'FakePlugin', 0, 0),
               (53, '2020-01-01', 0, 'German', 'de-DE', 1, 'FakePlugin', 0, 0),
               (54, '2020-01-01', 0, 'Ukrainian', 'uk-UA', 1, 'FakePlugin', 0, 0)");

        // Insert conflicting rows at IDs 37-39 for a DIFFERENT template (Id=1)
        await DbContext.Database.ExecuteSqlRawAsync(@"
        INSERT INTO MenuTemplateTranslations (Id, CreatedAt, Language, LocaleName, MenuTemplateId, Name)
        VALUES (37, '2020-01-01', 'English', 'en-US', 1, 'FakePlugin'),
               (38, '2020-01-01', 'Danish', 'da', 1, 'FakePlugin'),
               (39, '2020-01-01', 'German', 'de-DE', 1, 'FakePlugin')");

        // Act — re-run the idempotent CMS seeding SQL
        await DbContext.Database.ExecuteSqlRawAsync(IdempotentCmsSeedSql);

        // Assert — CMS translations were created (with auto-increment IDs, NOT 51-54)
        var cmsTranslations = await DbContext.MenuItemTranslations
            .AsNoTracking()
            .Where(t => t.MenuItemId == cmsMenuItem.Id)
            .ToListAsync();

        Assert.That(cmsTranslations.Count, Is.EqualTo(4),
            "CMS should have 4 translations even when IDs 51-54 are taken");
        Assert.That(cmsTranslations.Select(t => t.LocaleName),
            Is.EquivalentTo(new[] { "en-US", "da", "de-DE", "uk-UA" }));

        // Assert — the fake plugin translations at IDs 51-54 are untouched
        var fakeTranslations = await DbContext.MenuItemTranslations
            .AsNoTracking()
            .Where(t => t.Id >= 51 && t.Id <= 54)
            .ToListAsync();

        Assert.That(fakeTranslations.Count, Is.EqualTo(4), "Fake plugin rows should be untouched");
        Assert.That(fakeTranslations.All(t => t.Name == "FakePlugin"), Is.True);

        // Assert — CMS template translations exist
        var cmsTemplateTranslations = await DbContext.MenuTemplateTranslations
            .AsNoTracking()
            .Where(t => t.MenuTemplateId == cmsMenuItem.MenuTemplateId)
            .ToListAsync();

        Assert.That(cmsTemplateTranslations.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Migration_RunTwice_DoesNotDuplicateOrError()
    {
        // Arrange — seed initial CMS data (simulating the initial clean migration run)
        await DbContext.Database.ExecuteSqlRawAsync(IdempotentCmsSeedSql);

        var cmsMenuItem = await DbContext.MenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.E2EId == "cms");
        Assert.That(cmsMenuItem, Is.Not.Null);

        var translationCountBefore = await DbContext.MenuItemTranslations
            .AsNoTracking()
            .CountAsync(t => t.MenuItemId == cmsMenuItem.Id);

        var templateTranslationCountBefore = await DbContext.MenuTemplateTranslations
            .AsNoTracking()
            .CountAsync(t => t.MenuTemplateId == cmsMenuItem.MenuTemplateId);

        // Act — run the idempotent SQL a second time (should not error or duplicate)
        await DbContext.Database.ExecuteSqlRawAsync(IdempotentCmsSeedSql);

        // Assert — counts unchanged (no duplicates)
        var translationCountAfter = await DbContext.MenuItemTranslations
            .AsNoTracking()
            .CountAsync(t => t.MenuItemId == cmsMenuItem.Id);

        var templateTranslationCountAfter = await DbContext.MenuTemplateTranslations
            .AsNoTracking()
            .CountAsync(t => t.MenuTemplateId == cmsMenuItem.MenuTemplateId);

        Assert.That(translationCountAfter, Is.EqualTo(translationCountBefore),
            "Running migration SQL twice should not create duplicate MenuItemTranslations");
        Assert.That(templateTranslationCountAfter, Is.EqualTo(templateTranslationCountBefore),
            "Running migration SQL twice should not create duplicate MenuTemplateTranslations");
    }
}
