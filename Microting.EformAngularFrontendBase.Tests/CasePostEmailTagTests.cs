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
public class CasePostRecipientTests : DbTestFixture
{
    [Test]
    public async Task CasePostRecipient_Create_DoesCreate()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost { Name = Guid.NewGuid().ToString() };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostRecipient = new CasePostRecipient
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        // Act
        DbContext.CasePostRecipients.Add(casePostRecipient);
        await DbContext.SaveChangesAsync();

        var dbCasePostRecipient = await DbContext.CasePostRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbCasePostRecipient, Is.Not.Null);
        Assert.That(dbCasePostRecipient.Id, Is.EqualTo(casePostRecipient.Id));
        Assert.That(dbCasePostRecipient.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostRecipient.EmailTagId, Is.EqualTo(emailTag.Id));
    }

    [Test]
    public async Task CasePostRecipient_Update_DoesUpdate()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost1 = new CasePost { Name = Guid.NewGuid().ToString() };
        var casePost2 = new CasePost { Name = Guid.NewGuid().ToString() };
        DbContext.CasePosts.AddRange(casePost1, casePost2);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostRecipient = new CasePostRecipient
        {
            CasePostId = casePost1.Id,
            EmailTagId = emailTag.Id
        };

        DbContext.CasePostRecipients.Add(casePostRecipient);
        await DbContext.SaveChangesAsync();

        // Act
        casePostRecipient.CasePostId = casePost2.Id;
        await DbContext.SaveChangesAsync();

        var dbCasePostRecipient = await DbContext.CasePostRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbCasePostRecipient, Is.Not.Null);
        Assert.That(dbCasePostRecipient.Id, Is.EqualTo(casePostRecipient.Id));
        Assert.That(dbCasePostRecipient.CasePostId, Is.EqualTo(casePost2.Id));
    }

    [Test]
    public async Task CasePostRecipient_Delete_DoesDelete()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost { Name = Guid.NewGuid().ToString() };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostRecipient = new CasePostRecipient
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        DbContext.CasePostRecipients.Add(casePostRecipient);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.CasePostRecipients.Remove(casePostRecipient);
        await DbContext.SaveChangesAsync();

        var dbCasePostRecipients = await DbContext.CasePostRecipients.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbCasePostRecipients.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CasePostRecipient_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create CasePost and EmailTag first
        var casePost = new CasePost { Name = Guid.NewGuid().ToString() };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailTag = new EmailTag
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var casePostRecipient = new CasePostRecipient
        {
            CasePostId = casePost.Id,
            EmailTagId = emailTag.Id
        };

        DbContext.CasePostRecipients.Add(casePostRecipient);
        await DbContext.SaveChangesAsync();
        
        var expectedId = casePostRecipient.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbCasePostRecipient = await newContext.CasePostRecipients.AsNoTracking()
            .FirstOrDefaultAsync(etr => etr.CasePostId == casePost.Id && etr.EmailTagId == emailTag.Id);

        // Assert
        Assert.That(dbCasePostRecipient, Is.Not.Null);
        Assert.That(dbCasePostRecipient.Id, Is.EqualTo(expectedId));
        Assert.That(dbCasePostRecipient.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostRecipient.EmailTagId, Is.EqualTo(emailTag.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
