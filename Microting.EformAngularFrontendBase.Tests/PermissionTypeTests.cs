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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Permissions;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class PermissionTypeTests : DbTestFixture
{
    [Test]
    public async Task PermissionType_Create_DoesCreate()
    {
        // Arrange
        var permissionType = new PermissionType
        {
            Name = Guid.NewGuid().ToString()
        };

        // Act
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var dbPermissionType = await DbContext.PermissionTypes.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbPermissionType, Is.Not.Null);
        Assert.That(dbPermissionType.Id, Is.EqualTo(permissionType.Id));
        Assert.That(dbPermissionType.Name, Is.EqualTo(permissionType.Name));
        Assert.That(dbPermissionType.CreatedAt.ToString(), Is.EqualTo(permissionType.CreatedAt.ToString()));
    }

    [Test]
    public async Task PermissionType_Update_DoesUpdate()
    {
        // Arrange
        var permissionType = new PermissionType
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var oldName = permissionType.Name;

        // Act
        permissionType.Name = Guid.NewGuid().ToString();
        await DbContext.SaveChangesAsync();

        var dbPermissionType = await DbContext.PermissionTypes.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbPermissionType, Is.Not.Null);
        Assert.That(dbPermissionType.Id, Is.EqualTo(permissionType.Id));
        Assert.That(dbPermissionType.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbPermissionType.Name, Is.EqualTo(permissionType.Name));
    }

    [Test]
    public async Task PermissionType_Delete_DoesDelete()
    {
        // Arrange
        var permissionType = new PermissionType
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.PermissionTypes.Remove(permissionType);
        await DbContext.SaveChangesAsync();

        var dbPermissionTypes = await DbContext.PermissionTypes.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbPermissionTypes.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task PermissionType_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var permissionType = new PermissionType
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();
        
        var expectedId = permissionType.Id;
        var expectedName = permissionType.Name;
        var expectedCreatedAt = permissionType.CreatedAt;
        var expectedUpdatedAt = permissionType.UpdatedAt;

        // Act - Create a new context to test AsNoTracking behavior
        await using var newContext = GetContext(ConnectionString);
        var dbPermissionType = await newContext.PermissionTypes.AsNoTracking().FirstOrDefaultAsync(p => p.Name == expectedName);

        // Assert - AsNoTracking should return same values as we stored
        Assert.That(dbPermissionType, Is.Not.Null);
        Assert.That(dbPermissionType.Id, Is.EqualTo(expectedId));
        Assert.That(dbPermissionType.Name, Is.EqualTo(expectedName));
        Assert.That(dbPermissionType.CreatedAt.ToString(), Is.EqualTo(expectedCreatedAt.ToString()));
        Assert.That(dbPermissionType.UpdatedAt?.ToString(), Is.EqualTo(expectedUpdatedAt?.ToString()));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
