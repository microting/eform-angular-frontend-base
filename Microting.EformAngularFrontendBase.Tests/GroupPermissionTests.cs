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
public class GroupPermissionTests : DbTestFixture
{
    [Test]
    public async Task GroupPermission_Create_DoesCreate()
    {
        // Arrange - Create Permission and SecurityGroup first
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

        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var groupPermission = new GroupPermission
        {
            PermissionId = permission.Id,
            SecurityGroupId = securityGroup.Id
        };

        // Act
        DbContext.GroupPermissions.Add(groupPermission);
        await DbContext.SaveChangesAsync();

        var dbGroupPermission = await DbContext.GroupPermissions.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbGroupPermission, Is.Not.Null);
        Assert.That(dbGroupPermission.Id, Is.EqualTo(groupPermission.Id));
        Assert.That(dbGroupPermission.PermissionId, Is.EqualTo(permission.Id));
        Assert.That(dbGroupPermission.SecurityGroupId, Is.EqualTo(securityGroup.Id));
    }

    [Test]
    public async Task GroupPermission_Update_DoesUpdate()
    {
        // Arrange - Create Permission and SecurityGroup first
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

        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var groupPermission = new GroupPermission
        {
            PermissionId = permission1.Id,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.GroupPermissions.Add(groupPermission);
        await DbContext.SaveChangesAsync();

        var groupPermissionId = groupPermission.Id;

        // Act
        groupPermission.PermissionId = permission2.Id;
        await DbContext.SaveChangesAsync();

        var dbGroupPermission = await DbContext.GroupPermissions.AsNoTracking()
            .FirstAsync(gp => gp.Id == groupPermissionId);

        // Assert
        Assert.That(dbGroupPermission, Is.Not.Null);
        Assert.That(dbGroupPermission.Id, Is.EqualTo(groupPermission.Id));
        Assert.That(dbGroupPermission.PermissionId, Is.EqualTo(permission2.Id));
    }

    [Test]
    public async Task GroupPermission_Delete_DoesDelete()
    {
        // Arrange - Create Permission and SecurityGroup first
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

        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var groupPermission = new GroupPermission
        {
            PermissionId = permission.Id,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.GroupPermissions.Add(groupPermission);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.GroupPermissions.Remove(groupPermission);
        await DbContext.SaveChangesAsync();

        var dbGroupPermissions = await DbContext.GroupPermissions.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbGroupPermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task GroupPermission_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create Permission and SecurityGroup first
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

        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var groupPermission = new GroupPermission
        {
            PermissionId = permission.Id,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.GroupPermissions.Add(groupPermission);
        await DbContext.SaveChangesAsync();
        
        var expectedId = groupPermission.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbGroupPermission = await newContext.GroupPermissions.AsNoTracking()
            .FirstOrDefaultAsync(gp => gp.PermissionId == permission.Id && gp.SecurityGroupId == securityGroup.Id);

        // Assert
        Assert.That(dbGroupPermission, Is.Not.Null);
        Assert.That(dbGroupPermission.Id, Is.EqualTo(expectedId));
        Assert.That(dbGroupPermission.PermissionId, Is.EqualTo(permission.Id));
        Assert.That(dbGroupPermission.SecurityGroupId, Is.EqualTo(securityGroup.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
