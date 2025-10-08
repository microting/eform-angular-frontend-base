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
public class EmailTagRecipientTests : DbTestFixture
{
    [Test]
    public async Task EmailTagRecipient_Create_DoesCreate()
    {
        // Arrange - Create EmailTag and EmailRecipient first
        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var emailTagRecipient = new EmailTagRecipient
        {
            EmailTagId = emailTag.Id,
            EmailRecipientId = emailRecipient.Id
        };

        // Act
        DbContext.EmailTagRecipients.Add(emailTagRecipient);
        await DbContext.SaveChangesAsync();

        var dbEmailTagRecipient = await DbContext.EmailTagRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEmailTagRecipient, Is.Not.Null);
        Assert.That(dbEmailTagRecipient.Id, Is.EqualTo(emailTagRecipient.Id));
        Assert.That(dbEmailTagRecipient.EmailTagId, Is.EqualTo(emailTag.Id));
        Assert.That(dbEmailTagRecipient.EmailRecipientId, Is.EqualTo(emailRecipient.Id));
    }

    [Test]
    public async Task EmailTagRecipient_Update_DoesUpdate()
    {
        // Arrange - Create EmailTag and EmailRecipient first
        var emailTag1 = new EmailTag { Name = Guid.NewGuid().ToString() };
        var emailTag2 = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.AddRange(emailTag1, emailTag2);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var emailTagRecipient = new EmailTagRecipient
        {
            EmailTagId = emailTag1.Id,
            EmailRecipientId = emailRecipient.Id
        };

        DbContext.EmailTagRecipients.Add(emailTagRecipient);
        await DbContext.SaveChangesAsync();

        var emailTagRecipientId = emailTagRecipient.Id;

        // Act
        emailTagRecipient.EmailTagId = emailTag2.Id;
        await DbContext.SaveChangesAsync();

        var dbEmailTagRecipient = await DbContext.EmailTagRecipients.AsNoTracking()
            .FirstAsync(etr => etr.Id == emailTagRecipientId);

        // Assert
        Assert.That(dbEmailTagRecipient, Is.Not.Null);
        Assert.That(dbEmailTagRecipient.Id, Is.EqualTo(emailTagRecipient.Id));
        Assert.That(dbEmailTagRecipient.EmailTagId, Is.EqualTo(emailTag2.Id));
    }

    [Test]
    public async Task EmailTagRecipient_Delete_DoesDelete()
    {
        // Arrange - Create EmailTag and EmailRecipient first
        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var emailTagRecipient = new EmailTagRecipient
        {
            EmailTagId = emailTag.Id,
            EmailRecipientId = emailRecipient.Id
        };

        DbContext.EmailTagRecipients.Add(emailTagRecipient);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EmailTagRecipients.Remove(emailTagRecipient);
        await DbContext.SaveChangesAsync();

        var dbEmailTagRecipients = await DbContext.EmailTagRecipients.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEmailTagRecipients.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EmailTagRecipient_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create EmailTag and EmailRecipient first
        var emailTag = new EmailTag { Name = Guid.NewGuid().ToString() };
        DbContext.EmailTags.Add(emailTag);
        await DbContext.SaveChangesAsync();

        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var emailTagRecipient = new EmailTagRecipient
        {
            EmailTagId = emailTag.Id,
            EmailRecipientId = emailRecipient.Id
        };

        DbContext.EmailTagRecipients.Add(emailTagRecipient);
        await DbContext.SaveChangesAsync();
        
        var expectedId = emailTagRecipient.Id;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEmailTagRecipient = await newContext.EmailTagRecipients.AsNoTracking()
            .FirstOrDefaultAsync(etr => etr.EmailTagId == emailTag.Id && etr.EmailRecipientId == emailRecipient.Id);

        // Assert
        Assert.That(dbEmailTagRecipient, Is.Not.Null);
        Assert.That(dbEmailTagRecipient.Id, Is.EqualTo(expectedId));
        Assert.That(dbEmailTagRecipient.EmailTagId, Is.EqualTo(emailTag.Id));
        Assert.That(dbEmailTagRecipient.EmailRecipientId, Is.EqualTo(emailRecipient.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
