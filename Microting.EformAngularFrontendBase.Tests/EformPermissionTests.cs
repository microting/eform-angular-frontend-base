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
public class EformPermissionTests : DbTestFixture
{
    [Test]
    public async Task EformPermission_Create_DoesCreate()
    {
        // Arrange - Create required entities
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup { TemplateId = 1, SecurityGroupId = securityGroup.Id };
        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

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

        var eformPermission = new EformPermission
        {
            PermissionId = permission.Id,
            EformInGroupId = eformInGroup.Id
        };

        // Act
        DbContext.EformPermissions.Add(eformPermission);
        await DbContext.SaveChangesAsync();

        var dbEformPermission = await DbContext.EformPermissions.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformPermission, Is.Not.Null);
        Assert.That(dbEformPermission.Id, Is.EqualTo(eformPermission.Id));
        Assert.That(dbEformPermission.PermissionId, Is.EqualTo(permission.Id));
        Assert.That(dbEformPermission.EformInGroupId, Is.EqualTo(eformInGroup.Id));
    }

    [Test]
    public async Task EformPermission_Update_DoesUpdate()
    {
        // Arrange - Create required entities
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup { TemplateId = 1, SecurityGroupId = securityGroup.Id };
        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

        var permissionType = new PermissionType { Name = Guid.NewGuid().ToString() };
        DbContext.PermissionTypes.Add(permissionType);
        await DbContext.SaveChangesAsync();

        var permission1 = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };
        var permission2 = new Permission
        {
            PermissionName = Guid.NewGuid().ToString(),
            ClaimName = Guid.NewGuid().ToString(),
            PermissionTypeId = permissionType.Id
        };
        DbContext.Permissions.AddRange(permission1, permission2);
        await DbContext.SaveChangesAsync();

        var eformPermission = new EformPermission
        {
            PermissionId = permission1.Id,
            EformInGroupId = eformInGroup.Id
        };

        DbContext.EformPermissions.Add(eformPermission);
        await DbContext.SaveChangesAsync();

        var eformPermissionId = eformPermission.Id;

        // Act
        eformPermission.PermissionId = permission2.Id;
        await DbContext.SaveChangesAsync();

        var dbEformPermission = await DbContext.EformPermissions.AsNoTracking()
            .FirstAsync(ep => ep.Id == eformPermissionId);

        // Assert
        Assert.That(dbEformPermission, Is.Not.Null);
        Assert.That(dbEformPermission.Id, Is.EqualTo(eformPermission.Id));
        Assert.That(dbEformPermission.PermissionId, Is.EqualTo(permission2.Id));
    }

    [Test]
    public async Task EformPermission_Delete_DoesDelete()
    {
        // Arrange - Create required entities
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup { TemplateId = 1, SecurityGroupId = securityGroup.Id };
        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

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

        var eformPermission = new EformPermission
        {
            PermissionId = permission.Id,
            EformInGroupId = eformInGroup.Id
        };

        DbContext.EformPermissions.Add(eformPermission);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformPermissions.Remove(eformPermission);
        await DbContext.SaveChangesAsync();

        var dbEformPermissions = await DbContext.EformPermissions.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformPermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformPermission_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create required entities
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup { TemplateId = 1, SecurityGroupId = securityGroup.Id };
        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

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

        var eformPermission = new EformPermission
        {
            PermissionId = permission.Id,
            EformInGroupId = eformInGroup.Id
        };

        DbContext.EformPermissions.Add(eformPermission);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformPermission.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEformPermission = await newContext.EformPermissions.AsNoTracking()
            .FirstOrDefaultAsync(ep => ep.PermissionId == permission.Id && ep.EformInGroupId == eformInGroup.Id);

        // Assert
        Assert.That(dbEformPermission, Is.Not.Null);
        Assert.That(dbEformPermission.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformPermission.PermissionId, Is.EqualTo(permission.Id));
        Assert.That(dbEformPermission.EformInGroupId, Is.EqualTo(eformInGroup.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
