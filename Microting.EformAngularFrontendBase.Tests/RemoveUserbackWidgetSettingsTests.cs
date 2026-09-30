/*
The MIT License (MIT)

Copyright (c) 2007 - 2026 Microting A/S

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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Seed.SeedItems;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

/// <summary>
/// The Userback widget was removed from the product, so its two configuration rows must
/// neither be seeded nor survive a migrate of an existing database.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class RemoveUserbackWidgetSettingsTests : DbTestFixture
{
    private const string MigrationBeforeRemoval = "20260917085145_AddIsActiveToUser";

    private static readonly string[] UserbackKeys =
    {
        "ApplicationSettings:IsUserbackWidgetEnabled",
        "ApplicationSettings:UserbackToken"
    };

    private Task<string[]> UserbackKeysInDb() =>
        DbContext.ConfigurationValues.AsNoTracking()
            .Where(x => UserbackKeys.Contains(x.Id))
            .Select(x => x.Id)
            .ToArrayAsync();

    [Test]
    public void ConfigurationSeed_DoesNotContainUserbackSettings()
    {
        var seededUserbackKeys = ConfigurationSeed.Data
            .Select(x => x.Id)
            .Where(id => id.Contains("Userback"))
            .ToArray();

        Assert.That(seededUserbackKeys, Is.Empty);
    }

    [Test]
    public async Task Migrate_RemovesUserbackSettingsFromAnExistingDatabase()
    {
        var migrator = DbContext.GetService<IMigrator>();

        // Arrange - step back to the schema an existing installation has today
        await migrator.MigrateAsync(MigrationBeforeRemoval);
        Assert.That(await UserbackKeysInDb(), Is.EquivalentTo(UserbackKeys),
            "Down should restore both rows");

        var restoredToken = await DbContext.ConfigurationValues.AsNoTracking()
            .SingleAsync(x => x.Id == "ApplicationSettings:UserbackToken");
        Assert.That(restoredToken.Value, Is.Empty, "Down must not restore a token value");

        // Act
        await migrator.MigrateAsync();

        // Assert
        Assert.That(await UserbackKeysInDb(), Is.Empty);
    }
}
