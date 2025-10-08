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
public class EformConfigurationValueTests : DbTestFixture
{
    [Test]
    public async Task EformConfigurationValue_Create_DoesCreate()
    {
        // Arrange
        var configValue = new EformConfigurationValue
        {
            Id = Guid.NewGuid().ToString(),
            Value = "Test Value"
        };

        // Act
        DbContext.ConfigurationValues.Add(configValue);
        await DbContext.SaveChangesAsync();

        var dbConfigValue = await DbContext.ConfigurationValues.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbConfigValue, Is.Not.Null);
        Assert.That(dbConfigValue.Id, Is.EqualTo(configValue.Id));
        Assert.That(dbConfigValue.Value, Is.EqualTo(configValue.Value));
    }

    [Test]
    public async Task EformConfigurationValue_Update_DoesUpdate()
    {
        // Arrange
        var configValue = new EformConfigurationValue
        {
            Id = Guid.NewGuid().ToString(),
            Value = "Test Value"
        };

        DbContext.ConfigurationValues.Add(configValue);
        await DbContext.SaveChangesAsync();

        var oldValue = configValue.Value;

        // Act
        configValue.Value = "Updated Value";
        await DbContext.SaveChangesAsync();

        var dbConfigValue = await DbContext.ConfigurationValues.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbConfigValue, Is.Not.Null);
        Assert.That(dbConfigValue.Id, Is.EqualTo(configValue.Id));
        Assert.That(dbConfigValue.Value, Is.Not.EqualTo(oldValue));
        Assert.That(dbConfigValue.Value, Is.EqualTo("Updated Value"));
    }

    [Test]
    public async Task EformConfigurationValue_Delete_DoesDelete()
    {
        // Arrange
        var configValue = new EformConfigurationValue
        {
            Id = Guid.NewGuid().ToString(),
            Value = "Test Value"
        };

        DbContext.ConfigurationValues.Add(configValue);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.ConfigurationValues.Remove(configValue);
        await DbContext.SaveChangesAsync();

        var dbConfigValues = await DbContext.ConfigurationValues.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbConfigValues.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformConfigurationValue_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var configValue = new EformConfigurationValue
        {
            Id = Guid.NewGuid().ToString(),
            Value = "Test Value"
        };

        DbContext.ConfigurationValues.Add(configValue);
        await DbContext.SaveChangesAsync();
        
        var expectedId = configValue.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbConfigValue = await newContext.ConfigurationValues.AsNoTracking()
            .FirstOrDefaultAsync(cv => cv.Id == expectedId);

        // Assert
        Assert.That(dbConfigValue, Is.Not.Null);
        Assert.That(dbConfigValue.Id, Is.EqualTo(expectedId));
        Assert.That(dbConfigValue.Value, Is.EqualTo("Test Value"));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
