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
public class SecurityGroupTests : DbTestFixture
{
    [Test]
    public async Task SecurityGroup_Create_DoesCreate()
    {
        // Arrange
        var securityGroup = new SecurityGroup
        {
            Name = Guid.NewGuid().ToString(),
            RedirectLink = "/dashboard"
        };

        // Act
        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var dbSecurityGroup = await DbContext.SecurityGroups.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbSecurityGroup, Is.Not.Null);
        Assert.That(dbSecurityGroup.Id, Is.EqualTo(securityGroup.Id));
        Assert.That(dbSecurityGroup.Name, Is.EqualTo(securityGroup.Name));
        Assert.That(dbSecurityGroup.RedirectLink, Is.EqualTo(securityGroup.RedirectLink));
    }

    [Test]
    public async Task SecurityGroup_Update_DoesUpdate()
    {
        // Arrange
        var securityGroup = new SecurityGroup
        {
            Name = Guid.NewGuid().ToString(),
            RedirectLink = "/dashboard"
        };

        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        var oldName = securityGroup.Name;
        var securityGroupId = securityGroup.Id;

        // Act
        securityGroup.Name = Guid.NewGuid().ToString();
        securityGroup.RedirectLink = "/home";
        await DbContext.SaveChangesAsync();

        var dbSecurityGroup = await DbContext.SecurityGroups.AsNoTracking()
            .FirstAsync(sg => sg.Id == securityGroupId);

        // Assert
        Assert.That(dbSecurityGroup, Is.Not.Null);
        Assert.That(dbSecurityGroup.Id, Is.EqualTo(securityGroup.Id));
        Assert.That(dbSecurityGroup.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbSecurityGroup.Name, Is.EqualTo(securityGroup.Name));
        Assert.That(dbSecurityGroup.RedirectLink, Is.EqualTo("/home"));
    }

    [Test]
    public async Task SecurityGroup_Delete_DoesDelete()
    {
        // Arrange
        var securityGroup = new SecurityGroup
        {
            Name = Guid.NewGuid().ToString(),
            RedirectLink = "/dashboard"
        };

        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.SecurityGroups.Remove(securityGroup);
        await DbContext.SaveChangesAsync();

        var dbSecurityGroups = await DbContext.SecurityGroups.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbSecurityGroups.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task SecurityGroup_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var securityGroup = new SecurityGroup
        {
            Name = Guid.NewGuid().ToString(),
            RedirectLink = "/dashboard"
        };

        DbContext.SecurityGroups.Add(securityGroup);
        await DbContext.SaveChangesAsync();
        
        var expectedId = securityGroup.Id;
        var expectedName = securityGroup.Name;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbSecurityGroup = await newContext.SecurityGroups.AsNoTracking()
            .FirstOrDefaultAsync(sg => sg.Name == expectedName);

        // Assert
        Assert.That(dbSecurityGroup, Is.Not.Null);
        Assert.That(dbSecurityGroup.Id, Is.EqualTo(expectedId));
        Assert.That(dbSecurityGroup.Name, Is.EqualTo(expectedName));
        Assert.That(dbSecurityGroup.RedirectLink, Is.EqualTo("/dashboard"));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
