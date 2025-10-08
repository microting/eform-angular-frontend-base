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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Mailing;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class CasePostEmailTagTests : DbTestFixture
{
    [Test]
    public async Task CasePostEmailTag_Create_DoesCreate()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostEmailTag = new CasePostEmailTag
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        // Act
        DbContext.CasePostEmailTags.Add(casePostEmailTag);
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailTag = await DbContext.CasePostEmailTags.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbCasePostEmailTag, Is.Not.Null);
        Assert.That(dbCasePostEmailTag.Id, Is.EqualTo(casePostEmailTag.Id));
        Assert.That(dbCasePostEmailTag.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostEmailTag.EmailTagId, Is.EqualTo(emailTag.Id));
    }

    [Test]
    public async Task CasePostEmailTag_Update_DoesUpdate()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag1 = new EmailTag { Name = Guid.NewGuid().ToString() };
        var emailTag2 = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.AddRange(emailTag1, emailTag2);
        await DbContext.SaveChangesAsync();

        var casePostEmailTag = new CasePostEmailTag
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag1.Id
        };

        DbContext.CasePostEmailTags.Add(casePostEmailTag);
        await DbContext.SaveChangesAsync();

        var casePostEmailTagId = casePostEmailTag.Id;

        // Act
        casePostEmailTag.EmailTagId = emailTag2.Id;
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailTag = await DbContext.CasePostEmailTags.AsNoTracking()
            .FirstAsync(cpet => cpet.Id == casePostEmailTagId);

        // Assert
        Assert.That(dbCasePostEmailTag, Is.Not.Null);
        Assert.That(dbCasePostEmailTag.Id, Is.EqualTo(casePostEmailTag.Id));
        Assert.That(dbCasePostEmailTag.EmailTagId, Is.EqualTo(emailTag2.Id));
    }

    [Test]
    public async Task CasePostEmailTag_Delete_DoesDelete()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostEmailTag = new CasePostEmailTag
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        DbContext.CasePostEmailTags.Add(casePostEmailTag);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.CasePostEmailTags.Remove(casePostEmailTag);
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailTags = await DbContext.CasePostEmailTags.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbCasePostEmailTags.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CasePostEmailTag_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostEmailTag = new CasePostEmailTag
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        DbContext.CasePostEmailTags.Add(casePostEmailTag);
        await DbContext.SaveChangesAsync();
        
        var expectedId = casePostEmailTag.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbCasePostEmailTag = await newContext.CasePostEmailTags.AsNoTracking()
            .FirstOrDefaultAsync(cpet => cpet.CasePostId == casePost.Id && cpet.EmailTagId == emailTag.Id);

        // Assert
        Assert.That(dbCasePostEmailTag, Is.Not.Null);
        Assert.That(dbCasePostEmailTag.Id, Is.EqualTo(expectedId));
        Assert.That(dbCasePostEmailTag.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostEmailTag.EmailTagId, Is.EqualTo(emailTag.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
