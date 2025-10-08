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
public class EformInGroupTests : DbTestFixture
{
    [Test]
    public async Task EformInGroup_Create_DoesCreate()
    {
        // Arrange - Create SecurityGroup first
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup
        {
            TemplateId = 1,
            SecurityGroupId = securityGroup.Id
        };

        // Act
        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

        var dbEformInGroup = await DbContext.EformInGroups.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformInGroup, Is.Not.Null);
        Assert.That(dbEformInGroup.Id, Is.EqualTo(eformInGroup.Id));
        Assert.That(dbEformInGroup.TemplateId, Is.EqualTo(eformInGroup.TemplateId));
        Assert.That(dbEformInGroup.SecurityGroupId, Is.EqualTo(securityGroup.Id));
    }

    [Test]
    public async Task EformInGroup_Update_DoesUpdate()
    {
        // Arrange - Create SecurityGroup first
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup
        {
            TemplateId = 1,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroupId = eformInGroup.Id;

        // Act
        eformInGroup.TemplateId = 2;
        await DbContext.SaveChangesAsync();

        var dbEformInGroup = await DbContext.EformInGroups.AsNoTracking()
            .FirstAsync(eig => eig.Id == eformInGroupId);

        // Assert
        Assert.That(dbEformInGroup, Is.Not.Null);
        Assert.That(dbEformInGroup.Id, Is.EqualTo(eformInGroup.Id));
        Assert.That(dbEformInGroup.TemplateId, Is.EqualTo(2));
    }

    [Test]
    public async Task EformInGroup_Delete_DoesDelete()
    {
        // Arrange - Create SecurityGroup first
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup
        {
            TemplateId = 1,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformInGroups.Remove(eformInGroup);
        await DbContext.SaveChangesAsync();

        var dbEformInGroups = await DbContext.EformInGroups.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformInGroups.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformInGroup_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create SecurityGroup first
        var securityGroup = new SecurityGroup { Name = Guid.NewGuid().ToString() };
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var eformInGroup = new EformInGroup
        {
            TemplateId = 1,
            SecurityGroupId = securityGroup.Id
        };

        DbContext.EformInGroups.Add(eformInGroup);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformInGroup.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEformInGroup = await newContext.EformInGroups.AsNoTracking()
            .FirstOrDefaultAsync(eig => eig.TemplateId == 1 && eig.SecurityGroupId == securityGroup.Id);

        // Assert
        Assert.That(dbEformInGroup, Is.Not.Null);
        Assert.That(dbEformInGroup.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformInGroup.TemplateId, Is.EqualTo(1));
        Assert.That(dbEformInGroup.SecurityGroupId, Is.EqualTo(securityGroup.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
