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
public class EmailRecipientTests : DbTestFixture
{
    [Test]
    public async Task EmailRecipient_Create_DoesCreate()
    {
        // Arrange
        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };

        // Act
        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var dbEmailRecipient = await DbContext.EmailRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEmailRecipient, Is.Not.Null);
        Assert.That(dbEmailRecipient.Id, Is.EqualTo(emailRecipient.Id));
        Assert.That(dbEmailRecipient.Name, Is.EqualTo(emailRecipient.Name));
        Assert.That(dbEmailRecipient.Email, Is.EqualTo(emailRecipient.Email));
    }

    [Test]
    public async Task EmailRecipient_Update_DoesUpdate()
    {
        // Arrange
        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };

        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        var oldName = emailRecipient.Name;

        // Act
        emailRecipient.Name = Guid.NewGuid().ToString();
        emailRecipient.Email = $"{Guid.NewGuid()}@updated.com";
        await DbContext.SaveChangesAsync();

        var dbEmailRecipient = await DbContext.EmailRecipients.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEmailRecipient, Is.Not.Null);
        Assert.That(dbEmailRecipient.Id, Is.EqualTo(emailRecipient.Id));
        Assert.That(dbEmailRecipient.Name, Is.Not.EqualTo(oldName));
        Assert.That(dbEmailRecipient.Name, Is.EqualTo(emailRecipient.Name));
        Assert.That(dbEmailRecipient.Email, Is.EqualTo(emailRecipient.Email));
    }

    [Test]
    public async Task EmailRecipient_Delete_DoesDelete()
    {
        // Arrange
        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };

        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EmailRecipients.Remove(emailRecipient);
        await DbContext.SaveChangesAsync();

        var dbEmailRecipients = await DbContext.EmailRecipients.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEmailRecipients.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EmailRecipient_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var emailRecipient = new EmailRecipient
        {
            Name = Guid.NewGuid().ToString(),
            Email = $"{Guid.NewGuid()}@test.com"
        };

        DbContext.EmailRecipients.Add(emailRecipient);
        await DbContext.SaveChangesAsync();
        
        var expectedId = emailRecipient.Id;
        var expectedEmail = emailRecipient.Email;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEmailRecipient = await newContext.EmailRecipients.AsNoTracking()
            .FirstOrDefaultAsync(er => er.Email == expectedEmail);

        // Assert
        Assert.That(dbEmailRecipient, Is.Not.Null);
        Assert.That(dbEmailRecipient.Id, Is.EqualTo(expectedId));
        Assert.That(dbEmailRecipient.Email, Is.EqualTo(expectedEmail));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
