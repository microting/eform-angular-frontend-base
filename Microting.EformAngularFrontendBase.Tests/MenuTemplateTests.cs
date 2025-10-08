/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

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

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.EformAngularFrontendBase.Infrastructure.Data;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Menu;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MenuTemplateTests : DbTestFixture
{
    [Test]
    public async Task MenuTemplate_Create_DoesCreate()
    {
        // Arrange
        var menuTemplate = new MenuTemplate
        {
            Name = Guid.NewGuid().ToString(),
            DefaultLink = "/test",
            E2EId = Guid.NewGuid().ToString()
        };

        // Act
        DbContext.MenuTemplates.Add(menuTemplate);
        await DbContext.SaveChangesAsync();

        var dbMenuTemplate = await DbContext.MenuTemplates.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbMenuTemplate, Is.Not.Null);
        Assert.That(dbMenuTemplate.Id, Is.EqualTo(menuTemplate.Id));
        Assert.That(dbMenuTemplate.Name, Is.EqualTo(menuTemplate.Name));
        Assert.That(dbMenuTemplate.DefaultLink, Is.EqualTo(menuTemplate.DefaultLink));
        Assert.That(dbMenuTemplate.E2EId, Is.EqualTo(menuTemplate.E2EId));
        Assert.That(dbMenuTemplate.CreatedAt.ToString(), Is.EqualTo(menuTemplate.CreatedAt.ToString()));
    }

    [Test]
    public async Task MenuTemplate_Update_DoesUpdate()
    {
        // Arrange
        var menuTemplate = new MenuTemplate
        {
            Name = Guid.NewGuid().ToString(),
            DefaultLink = "/test",
            E2EId = Guid.NewGuid().ToString()
        };

        DbContext.MenuTemplates.Add(menuTemplate);
        await DbContext.SaveChangesAsync();

        var oldName = menuTemplate.Name;

        // Act
        menuTemplate.Name = Guid.NewGuid().ToString();
        menuTemplate.DefaultLink = "/updated";
        await DbContext.SaveChangesAsync();

        var dbMenuTemplate = await DbContext.MenuTemplates.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbMenuTemplate, Is.Not.Null);
        Assert.That(dbMenuTemplate.Id, Is.EqualTo(menuTemplate.Id));
        Assert.That(dbMenuTemplate.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbMenuTemplate.Name, Is.EqualTo(menuTemplate.Name));
        Assert.That(dbMenuTemplate.DefaultLink, Is.EqualTo("/updated"));
    }

    [Test]
    public async Task MenuTemplate_Delete_DoesDelete()
    {
        // Arrange
        var menuTemplate = new MenuTemplate
        {
            Name = Guid.NewGuid().ToString(),
            DefaultLink = "/test",
            E2EId = Guid.NewGuid().ToString()
        };

        DbContext.MenuTemplates.Add(menuTemplate);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.MenuTemplates.Remove(menuTemplate);
        await DbContext.SaveChangesAsync();

        var dbMenuTemplates = await DbContext.MenuTemplates.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbMenuTemplates.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task MenuTemplate_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var menuTemplate = new MenuTemplate
        {
            Name = Guid.NewGuid().ToString(),
            DefaultLink = "/test",
            E2EId = Guid.NewGuid().ToString()
        };

        DbContext.MenuTemplates.Add(menuTemplate);
        await DbContext.SaveChangesAsync();
        
        var expectedId = menuTemplate.Id;
        var expectedName = menuTemplate.Name;
        var expectedE2EId = menuTemplate.E2EId;

        // Act - Create a new context to test AsNoTracking behavior
        await using var newContext = GetContext(ConnectionString);
        var dbMenuTemplate = await newContext.MenuTemplates.AsNoTracking().FirstOrDefaultAsync(mt => mt.E2EId == expectedE2EId);

        // Assert - AsNoTracking should return same values as we stored
        Assert.That(dbMenuTemplate, Is.Not.Null);
        Assert.That(dbMenuTemplate.Id, Is.EqualTo(expectedId));
        Assert.That(dbMenuTemplate.Name, Is.EqualTo(expectedName));
        Assert.That(dbMenuTemplate.E2EId, Is.EqualTo(expectedE2EId));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
