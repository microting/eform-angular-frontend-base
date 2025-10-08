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
public class EformReportElementTests : DbTestFixture
{
    [Test]
    public async Task EformReportElement_Create_DoesCreate()
    {
        // Arrange - Create EformReport first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement
        {
            ElementId = 1,
            EformReportId = eformReport.Id
        };

        // Act
        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var dbEformReportElement = await DbContext.EformReportElements.AsNoTracking().FirstAsync();

        // Assert
        Assert.That(dbEformReportElement, Is.Not.Null);
        Assert.That(dbEformReportElement.Id, Is.EqualTo(eformReportElement.Id));
        Assert.That(dbEformReportElement.ElementId, Is.EqualTo(eformReportElement.ElementId));
        Assert.That(dbEformReportElement.EformReportId, Is.EqualTo(eformReport.Id));
    }

    [Test]
    public async Task EformReportElement_Update_DoesUpdate()
    {
        // Arrange - Create EformReport first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement
        {
            ElementId = 1,
            EformReportId = eformReport.Id
        };

        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        var oldElementId = eformReportElement.ElementId;
        var eformReportElementId = eformReportElement.Id;

        // Act
        eformReportElement.ElementId = 2;
        await DbContext.SaveChangesAsync();

        var dbEformReportElement = await DbContext.EformReportElements.AsNoTracking()
            .FirstAsync(ere => ere.Id == eformReportElementId);

        // Assert
        Assert.That(dbEformReportElement, Is.Not.Null);
        Assert.That(dbEformReportElement.Id, Is.EqualTo(eformReportElement.Id));
        Assert.That(dbEformReportElement.ElementId, Is.Not.EqualTo(oldElementId));
        Assert.That(dbEformReportElement.ElementId, Is.EqualTo(2));
    }

    [Test]
    public async Task EformReportElement_Delete_DoesDelete()
    {
        // Arrange - Create EformReport first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement
        {
            ElementId = 1,
            EformReportId = eformReport.Id
        };

        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();

        // Act
        DbContext.EformReportElements.Remove(eformReportElement);
        await DbContext.SaveChangesAsync();

        var dbEformReportElements = await DbContext.EformReportElements.AsNoTracking().ToListAsync();

        // Assert
        Assert.That(dbEformReportElements.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task EformReportElement_AsNoTracking_ReturnsCorrectValues()
    {
        // Arrange - Create EformReport first
        var eformReport = new EformReport { TemplateId = 1, Description = "Test Report" };
        DbContext.EformReports.Add(eformReport);
        await DbContext.SaveChangesAsync();

        var eformReportElement = new EformReportElement
        {
            ElementId = 1,
            EformReportId = eformReport.Id
        };

        DbContext.EformReportElements.Add(eformReportElement);
        await DbContext.SaveChangesAsync();
        
        var expectedId = eformReportElement.Id;
        var expectedElementId = eformReportElement.ElementId;

        // Act
        await using var newContext = GetContext(ConnectionString);
        var dbEformReportElement = await newContext.EformReportElements.AsNoTracking()
            .FirstOrDefaultAsync(ere => ere.ElementId == expectedElementId && ere.EformReportId == eformReport.Id);

        // Assert
        Assert.That(dbEformReportElement, Is.Not.Null);
        Assert.That(dbEformReportElement.Id, Is.EqualTo(expectedId));
        Assert.That(dbEformReportElement.ElementId, Is.EqualTo(expectedElementId));
        Assert.That(dbEformReportElement.EformReportId, Is.EqualTo(eformReport.Id));
    }
    
    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        return new BaseDbContext(optionsBuilder.Options);
    }
}
