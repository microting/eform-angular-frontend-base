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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Mailing;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class EmailTagTests : DbTestFixture
{
    [Test]
    public async Task EmailTag_Create_DoesCreate()
    {
        // Arrange
        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString()
        };

        // Act
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var dbEmailTag = await DbContext.EmailTags.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEmailTag, Is.Not.Null);
        Assert.That(dbEmailTag.Id, Is.EqualTo(emailTag.Id));
        Assert.That(dbEmailTag.Name, Is.EqualTo(emailTag.Name));
        Assert.That(dbEmailTag.CreatedAt.ToString(), Is.EqualTo(emailTag.CreatedAt.ToString()));
    }

    [Test]
    public async Task EmailTag_Update_DoesUpdate()
    {
        // Arrange
        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var oldName = emailTag.Name;
        var emailTagId = emailTag.Id;

        // Act
        emailTag.Name = Guid.NewGuid().ToString();
        await DbContext.SaveChangesAsync();

        var dbEmailTag = await DbContext.EmailTags.AsNoTracking()
            .FirstAsync(et => et.Id == emailTagId);

        // Assert
        Assert.That(dbEmailTag, Is.Not.Null);
        Assert.That(dbEmailTag.Id, Is.EqualTo(emailTag.Id));
        Assert.That(dbEmailTag.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbEmailTag.Name, Is.EqualTo(emailTag.Name));
    }

    [Test]
    public async Task EmailTag_Delete_DoesDelete()
    {
        // Arrange
        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EmailTags.Remove(emailTag);
        await DbContext.SaveChangesAsync();

        var dbEmailTags = await DbContext.EmailTags.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEmailTags.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EmailTag_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString()
        };

        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();
        
        var expectedId = emailTag.Id;
        var expectedName = emailTag.Name;

        // Act - Create a new context to test AsNoTracking behavior
        await using var newContext = GetContext(ConnectionString);
        var dbEmailTag = await newContext.EmailTags.AsNoTracking().FirstOrDefaultAsync(et => et.Name == expectedName);

        // Assert - AsNoTracking should return same values as we stored
        Assert.That(dbEmailTag, Is.Not.Null);
        Assert.That(dbEmailTag.Id, Is.EqualTo(expectedId));
        Assert.That(dbEmailTag.Name, Is.EqualTo(expectedName));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
