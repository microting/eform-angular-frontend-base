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
using Microting.eFormApi.BasePn.Infrastructure.Database.Entities;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

/// <summary>
/// The round-trip tests here exist because the mapping's HasDefaultValue(true) and the
/// "= true" initializer on EformUser.IsActive only behave correctly together, and getting
/// it wrong creates every new account disabled with nothing else to catch it. The remarks
/// on EformUser.IsActive explain why.
/// </summary>
[Parallelizable(ParallelScope.Fixtures)]
[TestFixture]
public class UserIsActiveTests : DbTestFixture
{
    private static EformUser NewUser() => new()
    {
        UserName = Guid.NewGuid().ToString(),
        Email = $"{Guid.NewGuid()}@example.com",
        FirstName = "Test",
        LastName = "User"
    };

    [Test]
    public void EformUser_IsActive_IsMappedWithADefaultOfTrue()
    {
        var property = DbContext.Model
            .FindEntityType(typeof(EformUser))
            ?.FindProperty(nameof(EformUser.IsActive));

        Assert.That(property, Is.Not.Null, "IsActive should be mapped on EformUser");
        Assert.That(property.IsNullable, Is.False, "IsActive should be non-nullable");
        Assert.That(property.GetDefaultValue(), Is.True,
            "IsActive should default to true, so existing and new accounts stay enabled");
    }

    [Test]
    public async Task EformUser_Create_IsActiveByDefault()
    {
        var user = NewUser();

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);

        Assert.That(stored.IsActive, Is.True, "a new account must be created enabled");
    }

    [Test]
    public async Task EformUser_CreatedDisabled_StaysDisabled()
    {
        var user = NewUser();
        user.IsActive = false;

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);

        Assert.That(stored.IsActive, Is.False,
            "an explicitly disabled account must not be re-enabled by the column default");
    }

    [Test]
    public async Task EformUser_Disable_DoesDisable()
    {
        var user = NewUser();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        user.IsActive = false;
        await DbContext.SaveChangesAsync();

        var stored = await DbContext.Users.AsNoTracking().FirstAsync(x => x.Id == user.Id);

        Assert.That(stored.IsActive, Is.False);
    }
}
