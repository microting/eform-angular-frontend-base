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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microting.EformAngularFrontendBase.Infrastructure.Data;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Menu;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class MenuItemTests : DbTestFixture
{
    [Test]
    public async Task MenuItem_Create_DoesCreate()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Name = Guid.NewGuid().ToString(),
            E2EId = Guid.NewGuid().ToString(),
            Link = "/test",
            Position = 1,
            IsInternalLink = true
        };

        // Act
        DbContext.MenuItems.Add(menuItem);
        await DbContext.SaveChangesAsync();

        var dbMenuItem = await DbContext.MenuItems.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbMenuItem, Is.Not.Null);
        Assert.That(dbMenuItem.Id, Is.EqualTo(menuItem.Id));
        Assert.That(dbMenuItem.Name, Is.EqualTo(menuItem.Name));
        Assert.That(dbMenuItem.E2EId, Is.EqualTo(menuItem.E2EId));
        Assert.That(dbMenuItem.Link, Is.EqualTo(menuItem.Link));
        Assert.That(dbMenuItem.Position, Is.EqualTo(menuItem.Position));
        Assert.That(dbMenuItem.IsInternalLink, Is.True);
    }

    [Test]
    public async Task MenuItem_Update_DoesUpdate()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Name = Guid.NewGuid().ToString(),
            E2EId = Guid.NewGuid().ToString(),
            Link = "/test",
            Position = 1
        };

        DbContext.MenuItems.Add(menuItem);
        await DbContext.SaveChangesAsync();

        var oldName = menuItem.Name;
        var menuItemId = menuItem.Id;

        // Act
        menuItem.Name = Guid.NewGuid().ToString();
        menuItem.Link = "/updated";
        menuItem.Position = 2;
        await DbContext.SaveChangesAsync();

        var dbMenuItem = await DbContext.MenuItems.AsNoTracking()
            .FirstAsync(mi => mi.Id == menuItemId);

        // Assert
        Assert.That(dbMenuItem, Is.Not.Null);
        Assert.That(dbMenuItem.Id, Is.EqualTo(menuItem.Id));
        Assert.That(dbMenuItem.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbMenuItem.Name, Is.EqualTo(menuItem.Name));
        Assert.That(dbMenuItem.Link, Is.EqualTo("/updated"));
        Assert.That(dbMenuItem.Position, Is.EqualTo(2));
    }

    [Test]
    public async Task MenuItem_Delete_DoesDelete()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Name = Guid.NewGuid().ToString(),
            E2EId = Guid.NewGuid().ToString(),
            Link = "/test",
            Position = 1
        };

        DbContext.MenuItems.Add(menuItem);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.MenuItems.Remove(menuItem);
        await DbContext.SaveChangesAsync();

        var dbMenuItems = await DbContext.MenuItems.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbMenuItems.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task MenuItem_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var menuItem = new MenuItem
        {
            Name = Guid.NewGuid().ToString(),
            E2EId = Guid.NewGuid().ToString(),
            Link = "/test",
            Position = 1,
            IsInternalLink = true
        };

        DbContext.MenuItems.Add(menuItem);
        await DbContext.SaveChangesAsync();
        
        var expectedId = menuItem.Id;
        var expectedE2EId = menuItem.E2EId;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbMenuItem = await newContext.MenuItems.AsNoTracking()
            .FirstOrDefaultAsync(mi => mi.E2EId == expectedE2EId);

        // Assert
        Assert.That(dbMenuItem, Is.Not.Null);
        Assert.That(dbMenuItem.Id, Is.EqualTo(expectedId));
        Assert.That(dbMenuItem.E2EId, Is.EqualTo(expectedE2EId));
        Assert.That(dbMenuItem.IsInternalLink, Is.True);
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
