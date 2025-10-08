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
public class CasePostEmailRecipientTests : DbTestFixture
{
    [Test]
    public async Task CasePostEmailRecipient_Create_DoesCreate()
    {
        // Arrange - Create CasePost and EmailRecipient first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var casePostEmailRecipient = new CasePostEmailRecipient
        {
            CasePostId = casePost.Id,
            EmailRecipientId = emailRecipient.Id
        };

        // Act
        DbContext.CasePostEmailRecipients.Add(casePostEmailRecipient);
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailRecipient = await DbContext.CasePostEmailRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbCasePostEmailRecipient, Is.Not.Null);
        Assert.That(dbCasePostEmailRecipient.Id, Is.EqualTo(casePostEmailRecipient.Id));
        Assert.That(dbCasePostEmailRecipient.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostEmailRecipient.EmailRecipientId, Is.EqualTo(emailRecipient.Id));
    }

    [Test]
    public async Task CasePostEmailRecipient_Update_DoesUpdate()
    {
        // Arrange - Create CasePost and EmailRecipient first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailRecipient1 = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        var emailRecipient2 = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.AddRange(emailRecipient1, emailRecipient2);
        await DbContext.SaveChangesAsync();

        var casePostEmailRecipient = new CasePostEmailRecipient
        {
            CasePostId = casePost.Id,
            EmailRecipientId = emailRecipient1.Id
        };

        DbContext.CasePostEmailRecipients.Add(casePostEmailRecipient);
        await DbContext.SaveChangesAsync();

        var casePostEmailRecipientId = casePostEmailRecipient.Id;

        // Act
        casePostEmailRecipient.EmailRecipientId = emailRecipient2.Id;
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailRecipient = await DbContext.CasePostEmailRecipients.AsNoTracking()
            .FirstAsync(cper => cper.Id == casePostEmailRecipientId);

        // Assert
        Assert.That(dbCasePostEmailRecipient, Is.Not.Null);
        Assert.That(dbCasePostEmailRecipient.Id, Is.EqualTo(casePostEmailRecipient.Id));
        Assert.That(dbCasePostEmailRecipient.EmailRecipientId, Is.EqualTo(emailRecipient2.Id));
    }

    [Test]
    public async Task CasePostEmailRecipient_Delete_DoesDelete()
    {
        // Arrange - Create CasePost and EmailRecipient first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var casePostEmailRecipient = new CasePostEmailRecipient
        {
            CasePostId = casePost.Id,
            EmailRecipientId = emailRecipient.Id
        };

        DbContext.CasePostEmailRecipients.Add(casePostEmailRecipient);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.CasePostEmailRecipients.Remove(casePostEmailRecipient);
        await DbContext.SaveChangesAsync();

        var dbCasePostEmailRecipients = await DbContext.CasePostEmailRecipients.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbCasePostEmailRecipients.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task CasePostEmailRecipient_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create CasePost and EmailRecipient first
        var casePost = new CasePost
        {
            CaseId = 1,
            PostDate = DateTime.Now,
            Subject = "Test Subject",
            Text = "Test Text"
        };
        DbContext.CasePosts.Add(casePost);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var casePostEmailRecipient = new CasePostEmailRecipient
        {
            CasePostId = casePost.Id,
            EmailRecipientId = emailRecipient.Id
        };

        DbContext.CasePostEmailRecipients.Add(casePostEmailRecipient);
        await DbContext.SaveChangesAsync();
        
        var expectedId = casePostEmailRecipient.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbCasePostEmailRecipient = await newContext.CasePostEmailRecipients.AsNoTracking()
            .FirstOrDefaultAsync(cper => cper.CasePostId == casePost.Id && cper.EmailRecipientId == emailRecipient.Id);

        // Assert
        Assert.That(dbCasePostEmailRecipient, Is.Not.Null);
        Assert.That(dbCasePostEmailRecipient.Id, Is.EqualTo(expectedId));
        Assert.That(dbCasePostEmailRecipient.CasePostId, Is.EqualTo(casePost.Id));
        Assert.That(dbCasePostEmailRecipient.EmailRecipientId, Is.EqualTo(emailRecipient.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
