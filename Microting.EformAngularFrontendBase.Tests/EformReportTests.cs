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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Reports;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class EformReportTests : DbTestFixture
{
    [Test]
    public async Task EformReport_Create_DoesCreate()
    {
        // Arrange
        var headerImage = new byte[] { 1, 2, 3, 4, 5 };
        var eformReport = new EformReport
        {
            TemplateId = 1,
            Description = "Test Report",
            HeaderImage = headerImage,
            HeaderVisibility = "visible",
            IsDateVisible = true,
            IsWorkerNameVisible = false
        };

        // Act
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var dbEformReport = await DbContext.EformReports.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformReport, Is.Not.Null);
        Assert.That(dbEformReport.Id, Is.EqualTo(eformReport.Id));
        Assert.That(dbEformReport.TemplateId, Is.EqualTo(eformReport.TemplateId));
        Assert.That(dbEformReport.Description, Is.EqualTo(eformReport.Description));
        Assert.That(dbEformReport.HeaderImage, Is.EqualTo(headerImage));
        Assert.That(dbEformReport.HeaderVisibility, Is.EqualTo(eformReport.HeaderVisibility));
        Assert.That(dbEformReport.IsDateVisible, Is.EqualTo(eformReport.IsDateVisible));
        Assert.That(dbEformReport.IsWorkerNameVisible, Is.EqualTo(eformReport.IsWorkerNameVisible));
    }

    [Test]
    public async Task EformReport_Update_DoesUpdate()
    {
        // Arrange
        var eformReport = new EformReport
        {
            TemplateId = 1,
            Description = "Test Report",
            IsDateVisible = true,
            IsWorkerNameVisible = false
        };

        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var oldDescription = eformReport.Description;

        // Act
        eformReport.Description = "Updated Report";
        eformReport.IsDateVisible = false;
        eformReport.HeaderVisibility = "hidden";
        await DbContext.SaveChangesAsync();

        var dbEformReport = await DbContext.EformReports.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformReport, Is.Not.Null);
        Assert.That(dbEformReport.Id, Is.EqualTo(eformReport.Id));
        Assert.That(dbEformReport.Description, Is.Not.EqualTo(oldDescription));
        Assert.That(dbEformReport.Description, Is.EqualTo("Updated Report"));
        Assert.That(dbEformReport.IsDateVisible, Is.False);
        Assert.That(dbEformReport.HeaderVisibility, Is.EqualTo("hidden"));
    }

    [Test]
    public async Task EformReport_Delete_DoesDelete()
    {
        // Arrange
        var eformReport = new EformReport
        {
            TemplateId = 1,
            Description = "Test Report",
            IsDateVisible = true
        };

        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformReports.Remove(eformReport);
        await DbContext.SaveChangesAsync();

        var dbEformReports = await DbContext.EformReports.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformReports.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformReport_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange
        var headerImage = new byte[] { 1, 2, 3, 4, 5 };
        var eformReport = new EformReport
        {
            TemplateId = 1,
            Description = "Test Report",
            HeaderImage = headerImage,
            HeaderVisibility = "visible",
            IsDateVisible = true,
            IsWorkerNameVisible = false
        };

        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformReport.Id;
        var expectedTemplateId = eformReport.TemplateId;
        var expectedDescription = eformReport.Description;

        // Act - Create a new context to test AsNoTracking behavior
        await using var newContext = GetContext(ConnectionString);
        var dbEformReport = await newContext.EformReports.AsNoTracking()
            .FirstOrDefaultAsync(r => r.TemplateId == expectedTemplateId && r.Description == expectedDescription);

        // Assert - AsNoTracking should return same values as we stored
        Assert.That(dbEformReport, Is.Not.Null);
        Assert.That(dbEformReport.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformReport.TemplateId, Is.EqualTo(expectedTemplateId));
        Assert.That(dbEformReport.Description, Is.EqualTo(expectedDescription));
        Assert.That(dbEformReport.HeaderImage, Is.EqualTo(headerImage));
        Assert.That(dbEformReport.HeaderVisibility, Is.EqualTo("visible"));
        Assert.That(dbEformReport.IsDateVisible, Is.True);
        Assert.That(dbEformReport.IsWorkerNameVisible, Is.False);
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
