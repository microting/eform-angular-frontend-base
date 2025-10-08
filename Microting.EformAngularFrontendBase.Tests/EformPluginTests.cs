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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class EformPluginTests : DbTestFixture
{
    [Test]
    public async Task EformPlugin_Create_DoesCreate()
    {
        // Arrange
        var eformPlugin = new EformPlugin
        {
            PluginId = Guid.NewGuid().ToString(),
            ConnectionString = "Server=localhost;Database=test",
            Status = 1
        };

        // Act
        DbContext.EformPlugins.Add(eformPlugin);
        await DbContext.SaveChangesAsync();

        var dbEformPlugin = await DbContext.EformPlugins.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformPlugin, Is.Not.Null);
        Assert.That(dbEformPlugin.Id, Is.EqualTo(eformPlugin.Id));
        Assert.That(dbEformPlugin.PluginId, Is.EqualTo(eformPlugin.PluginId));
        Assert.That(dbEformPlugin.ConnectionString, Is.EqualTo(eformPlugin.ConnectionString));
        Assert.That(dbEformPlugin.Status, Is.EqualTo(eformPlugin.Status));
    }

    [Test]
    public async Task EformPlugin_Update_DoesUpdate()
    {
        // Arrange
        var eformPlugin = new EformPlugin
        {
            PluginId = Guid.NewGuid().ToString(),
            ConnectionString = "Server=localhost;Database=test",
            Status = 1
        };

        DbContext.EformPlugins.Add(eformPlugin);
        await DbContext.SaveChangesAsync();

        var oldPluginId = eformPlugin.PluginId;

        // Act
        eformPlugin.PluginId = Guid.NewGuid().ToString();
        eformPlugin.Status = 2;
        await DbContext.SaveChangesAsync();

        var dbEformPlugin = await DbContext.EformPlugins.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformPlugin, Is.Not.Null);
        Assert.That(dbEformPlugin.Id, Is.EqualTo(eformPlugin.Id));
        Assert.That(dbEformPlugin.PluginId, Is.Not.EqualTo(oldPluginId));
        Assert.That(dbEformPlugin.PluginId, Is.EqualTo(eformPlugin.PluginId));
        Assert.That(dbEformPlugin.Status, Is.EqualTo(2));
    }

    [Test]
    public async Task EformPlugin_Delete_DoesDelete()
    {
        // Arrange
        var eformPlugin = new EformPlugin
        {
            PluginId = Guid.NewGuid().ToString(),
            ConnectionString = "Server=localhost;Database=test",
            Status = 1
        };

        DbContext.EformPlugins.Add(eformPlugin);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformPlugins.Remove(eformPlugin);
        await DbContext.SaveChangesAsync();

        var dbEformPlugins = await DbContext.EformPlugins.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformPlugins.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformPlugin_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var eformPlugin = new EformPlugin
        {
            PluginId = Guid.NewGuid().ToString(),
            ConnectionString = "Server=localhost;Database=test",
            Status = 1
        };

        DbContext.EformPlugins.Add(eformPlugin);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformPlugin.Id;
        var expectedPluginId = eformPlugin.PluginId;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEformPlugin = await newContext.EformPlugins.AsNoTracking()
            .FirstOrDefaultAsync(p => p.PluginId == expectedPluginId);

        // Assert
        Assert.That(dbEformPlugin, Is.Not.Null);
        Assert.That(dbEformPlugin.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformPlugin.PluginId, Is.EqualTo(expectedPluginId));
        Assert.That(dbEformPlugin.Status, Is.EqualTo(1));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
