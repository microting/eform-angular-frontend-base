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
using NUnit.Framework;
using Testcontainers.MariaDb;

namespace Microting.EformAngularFrontendBase.Tests;

[TestFixture]
public abstract class DbTestFixture
{
#pragma warning disable NUnit1032
    private readonly MariaDbContainer _mariadbTestcontainer = new MariaDbBuilder()
        .WithDatabase("eform-angular-test")
        .WithUsername("root")
        .WithPassword("secretpassword")
        .WithImage("mariadb:10.8")
        .Build();
#pragma warning restore NUnit1032

    protected BaseDbContext DbContext;
    protected string ConnectionString;

    private BaseDbContext GetContext(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseMySql(connectionStr, new MariaDbServerVersion(
            ServerVersion.AutoDetect(connectionStr)));
        
        var dbContext = new BaseDbContext(optionsBuilder.Options);
        dbContext.Database.Migrate();
        
        return dbContext;
    }

    [SetUp]
    public async Task Setup()
    {
        Console.WriteLine($"{DateTime.Now} : Starting MariaDb Container...");
        await _mariadbTestcontainer.StartAsync();
        Console.WriteLine($"{DateTime.Now} : Started MariaDb Container");
        
        ConnectionString = _mariadbTestcontainer.GetConnectionString();
        DbContext = GetContext(ConnectionString);
        DbContext.Database.SetCommandTimeout(300);

        await DoSetup();
    }

    [TearDown]
    public async Task TearDown()
    {
        Console.WriteLine($"{DateTime.Now} : TearDown...");
        await ClearDb();
        await DbContext.DisposeAsync();
        await _mariadbTestcontainer.StopAsync();
    }

    private async Task ClearDb()
    {
        Console.WriteLine($"{DateTime.Now} : ClearDb...");
        
        // Clear all tables in reverse order of dependencies
        var tableNames = new[]
        {
            "MenuItemTranslations",
            "MenuTemplateTranslations",
            "MenuItemSecurityGroups",
            "MenuTemplatePermissions",
            "MenuItems",
            "MenuTemplates",
            "CasePostEmailRecipients",
            "CasePostEmailTags",
            "CasePosts",
            "EmailTagRecipients",
            "EmailRecipients",
            "EmailTags",
            "EformReportDataItems",
            "EformReportElements",
            "EformReports",
            "EformPermissions",
            "EformInGroups",
            "GroupPermissions",
            "SecurityGroupUsers",
            "SecurityGroups",
            "Permissions",
            "PermissionTypes",
            "SavedTags",
            "EformPlugins",
            "EformConfigurationValues"
        };

        foreach (var tableName in tableNames)
        {
            try
            {
                await DbContext.Database.ExecuteSqlRawAsync(
                    $"SET FOREIGN_KEY_CHECKS = 0; TRUNCATE TABLE `{tableName}`; SET FOREIGN_KEY_CHECKS = 1;");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not truncate {tableName}: {ex.Message}");
            }
        }
    }

#pragma warning disable 1998
    public virtual async Task DoSetup()
    {
    }
#pragma warning restore 1998
}
