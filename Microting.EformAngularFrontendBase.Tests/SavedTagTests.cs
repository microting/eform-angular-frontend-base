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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class SavedTagTests : DbTestFixture
{
    [Test]
    public async Task SavedTag_Create_DoesCreate()
    {
        // Arrange
        var savedTag = new SavedTag
        {
            TagId = 1,
            TagName = Guid.NewGuid().ToString(),
            EformUserId = 1
        };

        // Act
        DbContext.SavedTags.Add(savedTag);
        await DbContext.SaveChangesAsync();

        var dbSavedTag = await DbContext.SavedTags.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbSavedTag, Is.Not.Null);
        Assert.That(dbSavedTag.Id, Is.EqualTo(savedTag.Id));
        Assert.That(dbSavedTag.TagId, Is.EqualTo(savedTag.TagId));
        Assert.That(dbSavedTag.TagName, Is.EqualTo(savedTag.TagName));
        Assert.That(dbSavedTag.EformUserId, Is.EqualTo(savedTag.EformUserId));
        Assert.That(dbSavedTag.CreatedAt.ToString(), Is.EqualTo(savedTag.CreatedAt.ToString()));
    }

    [Test]
    public async Task SavedTag_Update_DoesUpdate()
    {
        // Arrange
        var savedTag = new SavedTag
        {
            TagId = 1,
            TagName = Guid.NewGuid().ToString(),
            EformUserId = 1
        };

        DbContext.SavedTags.Add(savedTag);
        await DbContext.SaveChangesAsync();

        var oldTagName = savedTag.TagName;

        // Act
        savedTag.TagName = Guid.NewGuid().ToString();
        savedTag.TagId = 2;
        await DbContext.SaveChangesAsync();

        var dbSavedTag = await DbContext.SavedTags.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbSavedTag, Is.Not.Null);
        Assert.That(dbSavedTag.Id, Is.EqualTo(savedTag.Id));
        Assert.That(dbSavedTag.TagName, Is.Not.EqualTo(oldTagName));
        Assert.That(dbSavedTag.TagName, Is.EqualTo(savedTag.TagName));
        Assert.That(dbSavedTag.TagId, Is.EqualTo(2));
    }

    [Test]
    public async Task SavedTag_Delete_DoesDelete()
    {
        // Arrange
        var savedTag = new SavedTag
        {
            TagId = 1,
            TagName = Guid.NewGuid().ToString(),
            EformUserId = 1
        };

        DbContext.SavedTags.Add(savedTag);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.SavedTags.Remove(savedTag);
        await DbContext.SaveChangesAsync();

        var dbSavedTags = await DbContext.SavedTags.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbSavedTags.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task SavedTag_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var savedTag = new SavedTag
        {
            TagId = 1,
            TagName = Guid.NewGuid().ToString(),
            EformUserId = 1
        };

        DbContext.SavedTags.Add(savedTag);
        await DbContext.SaveChangesAsync();
        
        var expectedId = savedTag.Id;
        var expectedTagId = savedTag.TagId;
        var expectedTagName = savedTag.TagName;
        var expectedEformUserId = savedTag.EformUserId;

        // Act - Create a new context to test AsNoTracking behavior
        await using var newContext = GetContext(ConnectionString);
        var dbSavedTag = await newContext.SavedTags.AsNoTracking().FirstOrDefaultAsync(st => st.TagName == expectedTagName);

        // Assert - AsNoTracking should return same values as we stored
        Assert.That(dbSavedTag, Is.Not.Null);
        Assert.That(dbSavedTag.Id, Is.EqualTo(expectedId));
        Assert.That(dbSavedTag.TagId, Is.EqualTo(expectedTagId));
        Assert.That(dbSavedTag.TagName, Is.EqualTo(expectedTagName));
        Assert.That(dbSavedTag.EformUserId, Is.EqualTo(expectedEformUserId));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
