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

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

/// <summary>
/// ThemeMode replaces the DarkTheme checkbox with system/light/dark. The migration tests
/// matter most: a column default alone would move every existing account to "system", so
/// the backfill from DarkTheme is what keeps existing users seeing what they chose.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class UserThemeModeTests : DbTestFixture
{
    private const string MigrationBeforeThemeMode = "20260930102540_RemoveUserbackWidgetSettings";

    private static EformUser NewUser() => new()
    {
        UserName = Guid.NewGuid().ToString(),
        Email = $"{Guid.NewGuid()}@example.com",
        FirstName = "Test",
        LastName = "User"
    };

    // Raw SQL on purpose: at the pre-migration point the EF model already carries
    // ThemeMode but the schema does not, so an EF insert would name an unknown column.
    private Task InsertLegacyUser(string userName, bool darkTheme) =>
        DbContext.Database.ExecuteSqlRawAsync(
            "INSERT INTO Users (UserName, Email, FirstName, LastName, DarkTheme, AccessFailedCount, " +
            "EmailConfirmed, ExternalLoginEnabled, IsGoogleAuthenticatorEnabled, LockoutEnabled, " +
            "PhoneNumberConfirmed, TwoFactorEnabled, IsActive) VALUES " +
            $"('{userName}', '{userName}@example.com', 'Test', 'User', {(darkTheme ? 1 : 0)}, 0, 0, 0, 0, 0, 0, 0, 1)");

    private async Task<(string ThemeMode, bool DarkTheme)> ReadUser(string userName)
    {
        var stored = await DbContext.Users.AsNoTracking().SingleAsync(x => x.UserName == userName);
        return (stored.ThemeMode, stored.DarkTheme);
    }

    [Test]
    public void EformUser_ThemeMode_IsMappedWithADefaultOfSystem()
    {
        var property = DbContext.Model
            .FindEntityType(typeof(EformUser))
            ?.FindProperty(nameof(EformUser.ThemeMode));

        Assert.That(property, Is.Not.Null, "ThemeMode should be mapped on EformUser");
        Assert.That(property.IsNullable, Is.True, "ThemeMode should be nullable");
        Assert.That(property.GetDefaultValue(), Is.EqualTo("system"),
            "ThemeMode should default to system for new accounts");
    }

    [Test]
    public async Task EformUser_Create_DefaultsToSystem()
    {
        var user = NewUser();

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);

        Assert.That(stored.ThemeMode, Is.EqualTo("system"));
    }

    [Test]
    public async Task EformUser_CreatedWithLight_StaysLight()
    {
        var user = NewUser();
        user.ThemeMode = "light";

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);

        Assert.That(stored.ThemeMode, Is.EqualTo("light"),
            "an explicit value must not be dropped from the INSERT in favour of the column default");
    }

    [Test]
    public async Task Migration_BackfillsDarkThemeUsersAsDark()
    {
        var migrator = DbContext.GetService<IMigrator>();
        var darkUser = Guid.NewGuid().ToString();
        var lightUser = Guid.NewGuid().ToString();

        // Arrange - the schema an existing installation has, before ThemeMode exists
        await migrator.MigrateAsync(MigrationBeforeThemeMode);
        await InsertLegacyUser(darkUser, true);
        await InsertLegacyUser(lightUser, false);

        // Act
        await migrator.MigrateAsync();

        // Assert
        var dark = await ReadUser(darkUser);
        var light = await ReadUser(lightUser);

        Assert.That(dark.ThemeMode, Is.EqualTo("dark"));
        Assert.That(dark.DarkTheme, Is.True, "DarkTheme itself must be left unchanged");
        Assert.That(light.ThemeMode, Is.EqualTo("light"));
        Assert.That(light.DarkTheme, Is.False, "DarkTheme itself must be left unchanged");
    }

    [Test]
    public async Task Migration_DownThenUp_RoundTrips()
    {
        var migrator = DbContext.GetService<IMigrator>();

        await migrator.MigrateAsync();
        await migrator.MigrateAsync(MigrationBeforeThemeMode);
        await migrator.MigrateAsync();

        var user = NewUser();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);
        Assert.That(stored.ThemeMode, Is.EqualTo("system"),
            "the column should be present with its default after a down/up round trip");
    }
}
