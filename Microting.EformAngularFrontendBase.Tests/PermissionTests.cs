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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Permissions;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class PermissionTests : DbTestFixture
{
    [Test]
    public async Task Permission_Create_DoesCreate()
    {
        // Arrange - Create PermissionType first
        var permissionType = new PermissionType { Name = Guid.NewGuid().ToString() };
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var permission = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };

        // Act
        DbContext.Permissions.Add(permission);
        await DbContext.SaveChangesAsync();

        var dbPermission = await DbContext.Permissions.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbPermission, Is.Not.Null);
        Assert.That(dbPermission.Id, Is.EqualTo(permission.Id));
        Assert.That(dbPermission.PermissionName, Is.EqualTo(permission.PermissionName));
        Assert.That(dbPermission.ClaimName, Is.EqualTo(permission.ClaimName));
        Assert.That(dbPermission.PermissionTypeId, Is.EqualTo(permissionType.Id));
    }

    [Test]
    public async Task Permission_Update_DoesUpdate()
    {
        // Arrange - Create PermissionType first
        var permissionType = new PermissionType { Name = Guid.NewGuid().ToString() };
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var permission = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };

        DbContext.Permissions.Add(permission);
        await DbContext.SaveChangesAsync();

        var oldName = permission.PermissionName;
        var permissionId = permission.Id;

        // Act
        permission.PermissionName = Guid.NewGuid().ToString();
        permission.ClaimName = "updated_claim";
        await DbContext.SaveChangesAsync();

        var dbPermission = await DbContext.Permissions.AsNoTracking()
            .FirstAsync(p => p.Id == permissionId);

        // Assert
        Assert.That(dbPermission, Is.Not.Null);
        Assert.That(dbPermission.Id, Is.EqualTo(permission.Id));
        Assert.That(dbPermission.PermissionName, Is.Not.EqualTo(oldName));
        Assert.That(dbPermission.PermissionName, Is.EqualTo(permission.PermissionName));
        Assert.That(dbPermission.ClaimName, Is.EqualTo("updated_claim"));
    }

    [Test]
    public async Task Permission_Delete_DoesDelete()
    {
        // Arrange - Create PermissionType first
        var permissionType = new PermissionType { Name = Guid.NewGuid().ToString() };
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var permission = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };

        DbContext.Permissions.Add(permission);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.Permissions.Remove(permission);
        await DbContext.SaveChangesAsync();

        var dbPermissions = await DbContext.Permissions.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbPermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task Permission_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create PermissionType first
        var permissionType = new PermissionType { Name = Guid.NewGuid().ToString() };
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var permission = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };

        DbContext.Permissions.Add(permission);
        await DbContext.SaveChangesAsync();
        
        var expectedId = permission.Id;
        var expectedClaimName = permission.ClaimName;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbPermission = await newContext.Permissions.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ClaimName == expectedClaimName);

        // Assert
        Assert.That(dbPermission, Is.Not.Null);
        Assert.That(dbPermission.Id, Is.EqualTo(expectedId));
        Assert.That(dbPermission.ClaimName, Is.EqualTo(expectedClaimName));
        Assert.That(dbPermission.PermissionTypeId, Is.EqualTo(permissionType.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
