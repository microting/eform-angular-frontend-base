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
}
