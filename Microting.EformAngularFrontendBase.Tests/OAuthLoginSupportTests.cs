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

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microting.EformAngularFrontendBase.Infrastructure.Data;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[TestFixture]
public class OAuthLoginSupportTests
{
    private BaseDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BaseDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        _context = new BaseDbContext(options);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
    }

    [Test]
    public void EformUser_ShouldHaveOAuthLoginAttributes()
    {
        // Arrange & Act
        var userEntityType = _context.Model.FindEntityType("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser");
        
        // Assert
        Assert.That(userEntityType, Is.Not.Null, "EformUser entity should be configured");

        // Verify OAuth attributes exist
        var googleIdProperty = userEntityType.FindProperty("GoogleId");
        Assert.That(googleIdProperty, Is.Not.Null, "GoogleId property should be configured");
        Assert.That(googleIdProperty.IsNullable, Is.True, "GoogleId should be nullable");
        
        var facebookIdProperty = userEntityType.FindProperty("FacebookId");
        Assert.That(facebookIdProperty, Is.Not.Null, "FacebookId property should be configured");
        Assert.That(facebookIdProperty.IsNullable, Is.True, "FacebookId should be nullable");
        
        var googleEmailProperty = userEntityType.FindProperty("GoogleEmail");
        Assert.That(googleEmailProperty, Is.Not.Null, "GoogleEmail property should be configured");
        Assert.That(googleEmailProperty.IsNullable, Is.True, "GoogleEmail should be nullable");
        
        var facebookEmailProperty = userEntityType.FindProperty("FacebookEmail");
        Assert.That(facebookEmailProperty, Is.Not.Null, "FacebookEmail property should be configured");
        Assert.That(facebookEmailProperty.IsNullable, Is.True, "FacebookEmail should be nullable");
        
        var externalLoginEnabledProperty = userEntityType.FindProperty("ExternalLoginEnabled");
        Assert.That(externalLoginEnabledProperty, Is.Not.Null, "ExternalLoginEnabled property should be configured");
        Assert.That(externalLoginEnabledProperty.IsNullable, Is.False, "ExternalLoginEnabled should not be nullable");
        Assert.That(externalLoginEnabledProperty.GetDefaultValue(), Is.EqualTo(true), "ExternalLoginEnabled should default to true");
        
        var preferredLoginProviderProperty = userEntityType.FindProperty("PreferredLoginProvider");
        Assert.That(preferredLoginProviderProperty, Is.Not.Null, "PreferredLoginProvider property should be configured");
        Assert.That(preferredLoginProviderProperty.IsNullable, Is.True, "PreferredLoginProvider should be nullable");
        Assert.That(preferredLoginProviderProperty.GetMaxLength(), Is.EqualTo(50), "PreferredLoginProvider should have max length of 50");
    }

    [Test]
    public void EformUser_OAuthProperties_ShouldBeConfiguredProperly()
    {
        // Arrange & Act
        var userEntityType = _context.Model.FindEntityType("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser");
        
        // Assert - Just verify properties exist and basic configuration
        var googleIdProperty = userEntityType.FindProperty("GoogleId");
        Assert.That(googleIdProperty, Is.Not.Null, "GoogleId should be configured");
        Assert.That(googleIdProperty.IsNullable, Is.True, "GoogleId should be nullable");
        
        var facebookIdProperty = userEntityType.FindProperty("FacebookId");
        Assert.That(facebookIdProperty, Is.Not.Null, "FacebookId should be configured");
        Assert.That(facebookIdProperty.IsNullable, Is.True, "FacebookId should be nullable");
        
        var externalLoginEnabledProperty = userEntityType.FindProperty("ExternalLoginEnabled");
        Assert.That(externalLoginEnabledProperty, Is.Not.Null, "ExternalLoginEnabled should be configured");
        Assert.That(externalLoginEnabledProperty.IsNullable, Is.False, "ExternalLoginEnabled should not be nullable");
        Assert.That(externalLoginEnabledProperty.GetDefaultValue(), Is.EqualTo(true), "ExternalLoginEnabled should default to true");
        
        var preferredLoginProviderProperty = userEntityType.FindProperty("PreferredLoginProvider");
        Assert.That(preferredLoginProviderProperty, Is.Not.Null, "PreferredLoginProvider should be configured");
        Assert.That(preferredLoginProviderProperty.IsNullable, Is.True, "PreferredLoginProvider should be nullable");
        Assert.That(preferredLoginProviderProperty.GetMaxLength(), Is.EqualTo(50), "PreferredLoginProvider should have max length of 50");
    }
}