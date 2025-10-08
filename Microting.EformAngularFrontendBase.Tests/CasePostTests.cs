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
public class CasePostTests : DbTestFixture
{
    [Test]
    public async Task CasePost_Create_DoesCreate()
    {
        // Arrange
        var postDate = DateTime.Now;
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = postDate,
            Subject = "Test Subject",
            Text = "Test Text",
            LinkToCase = true,
            AttachPdf = false
        };

        // Act
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var dbCasePost = await DbContext.CasePosts.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbCasePost, Is.Not.Null);
        Assert.That(dbCasePost.Id, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePost.CaseId, Is.EqualTo(casePost.CaseId));
        Assert.That(dbCasePost.Subject, Is.EqualTo(casePost.Subject));
        Assert.That(dbCasePost.Text, Is.EqualTo(casePost.Text));
        Assert.That(dbCasePost.LinkToCase, Is.True);
        Assert.That(dbCasePost.AttachPdf, Is.False);
    }

    [Test]
    public async Task CasePost_Update_DoesUpdate()
    {
        // Arrange
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text",
            LinkToCase = true
        };

        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var oldSubject = casePost.Subject;
        var casePostId = casePost.Id;

        // Act
        casePost.Subject = "Updated Subject";
        casePost.Text = "Updated Text";
        casePost.LinkToCase = false;
        await DbContext.SaveChangesAsync();

        var dbCasePost = await DbContext.CasePosts.AsNoTracking()
            .FirstAsync(cp => cp.Id == casePostId);

        // Assert
        Assert.That(dbCasePost, Is.Not.Null);
        Assert.That(dbCasePost.Id, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePost.Subject, Is.Not.EqualTo(oldSubject));
        Assert.That(dbCasePost.Subject, Is.EqualTo("Updated Subject"));
        Assert.That(dbCasePost.Text, Is.EqualTo("Updated Text"));
        Assert.That(dbCasePost.LinkToCase, Is.False);
    }

    [Test]
    public async Task CasePost_Delete_DoesDelete()
    {
        // Arrange
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };

        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.CasePosts.Remove(casePost);
        await DbContext.SaveChangesAsync();

        var dbCasePosts = await DbContext.CasePosts.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbCasePosts.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CasePost_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text",
            LinkToCase = true,
            AttachPdf = false
        };

        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();
        
        var expectedId = casePost.Id;
        var expectedCaseId = casePost.CaseId;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbCasePost = await newContext.CasePosts.AsNoTracking()
            .FirstOrDefaultAsync(cp => cp.CaseId == expectedCaseId);

        // Assert
        Assert.That(dbCasePost, Is.Not.Null);
        Assert.That(dbCasePost.Id, Is.EqualTo(expectedId));
        Assert.That(dbCasePost.CaseId, Is.EqualTo(expectedCaseId));
        Assert.That(dbCasePost.Subject, Is.EqualTo("Test Subject"));
        Assert.That(dbCasePost.LinkToCase, Is.True);
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
