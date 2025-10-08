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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Reports;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class EformReportDataItemTests : DbTestFixture
{
    [Test]
    public async Task EformReportDataItem_Create_DoesCreate()
    {
        // Arrange - Create EformReport and EformReportElement first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement { ElementId = 1, EformReportId = eformReport.Id };
        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var eformReportDataItem = new EformReportDataItem
        {
            DataItemId = 1,
            Position = 0,
            Visibility = true,
            EformReportElementId = eformReportElement.Id
        };

        // Act
        DbContext.EformReportDataItems.Add(eformReportDataItem);
        await DbContext.SaveChangesAsync();

        var dbEformReportDataItem = await DbContext.EformReportDataItems.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformReportDataItem, Is.Not.Null);
        Assert.That(dbEformReportDataItem.Id, Is.EqualTo(eformReportDataItem.Id));
        Assert.That(dbEformReportDataItem.DataItemId, Is.EqualTo(eformReportDataItem.DataItemId));
        Assert.That(dbEformReportDataItem.Position, Is.EqualTo(eformReportDataItem.Position));
        Assert.That(dbEformReportDataItem.Visibility, Is.EqualTo(eformReportDataItem.Visibility));
    }

    [Test]
    public async Task EformReportDataItem_Update_DoesUpdate()
    {
        // Arrange - Create EformReport and EformReportElement first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement { ElementId = 1, EformReportId = eformReport.Id };
        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var eformReportDataItem = new EformReportDataItem
        {
            DataItemId = 1,
            Position = 0,
            Visibility = true,
            EformReportElementId = eformReportElement.Id
        };

        DbContext.EformReportDataItems.Add(eformReportDataItem);
        await DbContext.SaveChangesAsync();

        // Act
        eformReportDataItem.Position = 5;
        eformReportDataItem.Visibility = false;
        await DbContext.SaveChangesAsync();

        var dbEformReportDataItem = await DbContext.EformReportDataItems.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformReportDataItem, Is.Not.Null);
        Assert.That(dbEformReportDataItem.Id, Is.EqualTo(eformReportDataItem.Id));
        Assert.That(dbEformReportDataItem.Position, Is.EqualTo(5));
        Assert.That(dbEformReportDataItem.Visibility, Is.False);
    }

    [Test]
    public async Task EformReportDataItem_Delete_DoesDelete()
    {
        // Arrange - Create EformReport and EformReportElement first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement { ElementId = 1, EformReportId = eformReport.Id };
        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var eformReportDataItem = new EformReportDataItem
        {
            DataItemId = 1,
            Position = 0,
            Visibility = true,
            EformReportElementId = eformReportElement.Id
        };

        DbContext.EformReportDataItems.Add(eformReportDataItem);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformReportDataItems.Remove(eformReportDataItem);
        await DbContext.SaveChangesAsync();

        var dbEformReportDataItems = await DbContext.EformReportDataItems.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformReportDataItems.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformReportDataItem_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create EformReport and EformReportElement first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement { ElementId = 1, EformReportId = eformReport.Id };
        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var eformReportDataItem = new EformReportDataItem
        {
            DataItemId = 1,
            Position = 0,
            Visibility = true,
            EformReportElementId = eformReportElement.Id
        };

        DbContext.EformReportDataItems.Add(eformReportDataItem);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformReportDataItem.Id;
        var expectedDataItemId = eformReportDataItem.DataItemId;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEformReportDataItem = await newContext.EformReportDataItems.AsNoTracking()
            .FirstOrDefaultAsync(di => di.DataItemId == expectedDataItemId && di.EformReportElementId == eformReportElement.Id);

        // Assert
        Assert.That(dbEformReportDataItem, Is.Not.Null);
        Assert.That(dbEformReportDataItem.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformReportDataItem.DataItemId, Is.EqualTo(expectedDataItemId));
        Assert.That(dbEformReportDataItem.Position, Is.EqualTo(0));
        Assert.That(dbEformReportDataItem.Visibility, Is.True);
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
