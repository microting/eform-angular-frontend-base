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
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Mailing;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Menu;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Permissions;
using Microting.EformAngularFrontendBase.Infrastructure.Data.Entities.Reports;
using NUnit.Framework;

namespace Microting.EformAngularFrontendBase.Tests;

[TestFixture]
public class ModelTests
{
    #region EformConfigurationValue Tests

    [Test]
    public void EformConfigurationValue_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformConfigurationValue
        {
            Id = "test-id",
            Value = "test-value"
        };

        // Assert
        Assert.That(model.Id, Is.EqualTo("test-id"));
        Assert.That(model.Value, Is.EqualTo("test-value"));
    }

    #endregion

    #region EformPlugin Tests

    [Test]
    public void EformPlugin_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformPlugin
        {
            PluginId = "test-plugin",
            ConnectionString = "Server=localhost;",
            Status = 1
        };

        // Assert
        Assert.That(model.PluginId, Is.EqualTo("test-plugin"));
        Assert.That(model.ConnectionString, Is.EqualTo("Server=localhost;"));
        Assert.That(model.Status, Is.EqualTo(1));
    }

    [Test]
    public void EformPlugin_MenuTemplates_IsInitialized()
    {
        // Arrange & Act
        var model = new EformPlugin();

        // Assert
        Assert.That(model.MenuTemplates, Is.Not.Null);
        Assert.That(model.MenuTemplates.Count, Is.EqualTo(0));
    }

    #endregion

    #region SavedTag Tests

    [Test]
    public void SavedTag_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new SavedTag
        {
            TagId = 1,
            TagName = "Test Tag",
            EformUserId = 2
        };

        // Assert
        Assert.That(model.TagId, Is.EqualTo(1));
        Assert.That(model.TagName, Is.EqualTo("Test Tag"));
        Assert.That(model.EformUserId, Is.EqualTo(2));
    }

    #endregion

    #region Mailing Entity Tests

    [Test]
    public void CasePost_CanSetAndGetProperties()
    {
        // Arrange & Act
        var postDate = DateTime.Now;
        var model = new CasePost
        {
            CaseId = 1,
            PostDate = postDate,
            Subject = "Test Subject",
            Text = "Test Text",
            LinkToCase = true,
            AttachPdf = false
        };

        // Assert
        Assert.That(model.CaseId, Is.EqualTo(1));
        Assert.That(model.PostDate, Is.EqualTo(postDate));
        Assert.That(model.Subject, Is.EqualTo("Test Subject"));
        Assert.That(model.Text, Is.EqualTo("Test Text"));
        Assert.That(model.LinkToCase, Is.True);
        Assert.That(model.AttachPdf, Is.False);
    }

    [Test]
    public void CasePost_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new CasePost();

        // Assert
        Assert.That(model.Recipients, Is.Not.Null);
        Assert.That(model.Recipients.Count, Is.EqualTo(0));
        Assert.That(model.Tags, Is.Not.Null);
        Assert.That(model.Tags.Count, Is.EqualTo(0));
    }

    [Test]
    public void CasePostEmailRecipient_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new CasePostEmailRecipient
        {
            EmailRecipientId = 1,
            CasePostId = 2
        };

        // Assert
        Assert.That(model.EmailRecipientId, Is.EqualTo(1));
        Assert.That(model.CasePostId, Is.EqualTo(2));
    }

    [Test]
    public void CasePostEmailTag_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new CasePostEmailTag
        {
            EmailTagId = 1,
            CasePostId = 2
        };

        // Assert
        Assert.That(model.EmailTagId, Is.EqualTo(1));
        Assert.That(model.CasePostId, Is.EqualTo(2));
    }

    [Test]
    public void EmailRecipient_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EmailRecipient
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("John Doe"));
        Assert.That(model.Email, Is.EqualTo("john@example.com"));
    }

    [Test]
    public void EmailTag_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EmailTag
        {
            Name = "Important"
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Important"));
    }

    [Test]
    public void EmailTagRecipient_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EmailTagRecipient
        {
            EmailTagId = 1,
            EmailRecipientId = 2
        };

        // Assert
        Assert.That(model.EmailTagId, Is.EqualTo(1));
        Assert.That(model.EmailRecipientId, Is.EqualTo(2));
    }

    #endregion

    #region Menu Entity Tests

    [Test]
    public void MenuItem_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuItem
        {
            Name = "Test Menu",
            E2EId = "test-menu",
            Link = "/test",
            Position = 1,
            MenuTemplateId = 2,
            ParentId = 3,
            IsInternalLink = true,
            IconName = "icon-test"
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Test Menu"));
        Assert.That(model.E2EId, Is.EqualTo("test-menu"));
        Assert.That(model.Link, Is.EqualTo("/test"));
        Assert.That(model.Position, Is.EqualTo(1));
        Assert.That(model.MenuTemplateId, Is.EqualTo(2));
        Assert.That(model.ParentId, Is.EqualTo(3));
        Assert.That(model.IsInternalLink, Is.True);
        Assert.That(model.IconName, Is.EqualTo("icon-test"));
    }

    [Test]
    public void MenuItem_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new MenuItem();

        // Assert
        Assert.That(model.ChildItems, Is.Not.Null);
        Assert.That(model.ChildItems.Count, Is.EqualTo(0));
        Assert.That(model.Translations, Is.Not.Null);
        Assert.That(model.Translations.Count, Is.EqualTo(0));
        Assert.That(model.MenuItemSecurityGroups, Is.Not.Null);
        Assert.That(model.MenuItemSecurityGroups.Count, Is.EqualTo(0));
    }

    [Test]
    public void MenuItem_IsInternalLink_DefaultsToTrue()
    {
        // Arrange & Act
        var model = new MenuItem();

        // Assert
        Assert.That(model.IsInternalLink, Is.True);
    }

    [Test]
    public void MenuItemSecurityGroup_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuItemSecurityGroup
        {
            MenuItemId = 1,
            SecurityGroupId = 2
        };

        // Assert
        Assert.That(model.MenuItemId, Is.EqualTo(1));
        Assert.That(model.SecurityGroupId, Is.EqualTo(2));
    }

    [Test]
    public void MenuItemTranslation_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuItemTranslation
        {
            Name = "Test Menu",
            LocaleName = "en-US",
            Language = "English",
            MenuItemId = 1
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Test Menu"));
        Assert.That(model.LocaleName, Is.EqualTo("en-US"));
        Assert.That(model.Language, Is.EqualTo("English"));
        Assert.That(model.MenuItemId, Is.EqualTo(1));
    }

    [Test]
    public void MenuTemplate_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuTemplate
        {
            Name = "Test Template",
            DefaultLink = "/test",
            E2EId = "test-template",
            EformPluginId = 1
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Test Template"));
        Assert.That(model.DefaultLink, Is.EqualTo("/test"));
        Assert.That(model.E2EId, Is.EqualTo("test-template"));
        Assert.That(model.EformPluginId, Is.EqualTo(1));
    }

    [Test]
    public void MenuTemplate_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new MenuTemplate();

        // Assert
        Assert.That(model.MenuItems, Is.Not.Null);
        Assert.That(model.MenuItems.Count, Is.EqualTo(0));
        Assert.That(model.Translations, Is.Not.Null);
        Assert.That(model.Translations.Count, Is.EqualTo(0));
        Assert.That(model.MenuTemplatePermissions, Is.Not.Null);
        Assert.That(model.MenuTemplatePermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public void MenuTemplatePermission_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuTemplatePermission
        {
            MenuTemplateId = 1,
            PermissionId = 2
        };

        // Assert
        Assert.That(model.MenuTemplateId, Is.EqualTo(1));
        Assert.That(model.PermissionId, Is.EqualTo(2));
    }

    [Test]
    public void MenuTemplateTranslation_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new MenuTemplateTranslation
        {
            Name = "Test Template",
            LocaleName = "en-US",
            Language = "English",
            MenuTemplateId = 1
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Test Template"));
        Assert.That(model.LocaleName, Is.EqualTo("en-US"));
        Assert.That(model.Language, Is.EqualTo("English"));
        Assert.That(model.MenuTemplateId, Is.EqualTo(1));
    }

    #endregion

    #region Permission Entity Tests

    [Test]
    public void Permission_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new Permission
        {
            PermissionName = "Read",
            ClaimName = "read_claim",
            PermissionTypeId = 1
        };

        // Assert
        Assert.That(model.PermissionName, Is.EqualTo("Read"));
        Assert.That(model.ClaimName, Is.EqualTo("read_claim"));
        Assert.That(model.PermissionTypeId, Is.EqualTo(1));
    }

    [Test]
    public void Permission_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new Permission();

        // Assert
        Assert.That(model.GroupPermissions, Is.Not.Null);
        Assert.That(model.GroupPermissions.Count, Is.EqualTo(0));
        Assert.That(model.MenuTemplatePermissions, Is.Not.Null);
        Assert.That(model.MenuTemplatePermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public void PermissionType_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new PermissionType
        {
            Name = "Admin"
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Admin"));
    }

    [Test]
    public void GroupPermission_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new GroupPermission
        {
            PermissionId = 1,
            SecurityGroupId = 2
        };

        // Assert
        Assert.That(model.PermissionId, Is.EqualTo(1));
        Assert.That(model.SecurityGroupId, Is.EqualTo(2));
    }

    [Test]
    public void SecurityGroup_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new SecurityGroup
        {
            Name = "Test Group",
            RedirectLink = "/dashboard"
        };

        // Assert
        Assert.That(model.Name, Is.EqualTo("Test Group"));
        Assert.That(model.RedirectLink, Is.EqualTo("/dashboard"));
    }

    [Test]
    public void SecurityGroup_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new SecurityGroup();

        // Assert
        Assert.That(model.SecurityGroupUsers, Is.Not.Null);
        Assert.That(model.SecurityGroupUsers.Count, Is.EqualTo(0));
        Assert.That(model.EformsInGroup, Is.Not.Null);
        Assert.That(model.EformsInGroup.Count, Is.EqualTo(0));
        Assert.That(model.MenuItemSecurityGroups, Is.Not.Null);
        Assert.That(model.MenuItemSecurityGroups.Count, Is.EqualTo(0));
    }

    [Test]
    public void SecurityGroupUser_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new SecurityGroupUser
        {
            SecurityGroupId = 1,
            EformUserId = 2
        };

        // Assert
        Assert.That(model.SecurityGroupId, Is.EqualTo(1));
        Assert.That(model.EformUserId, Is.EqualTo(2));
    }

    [Test]
    public void EformInGroup_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformInGroup
        {
            TemplateId = 1,
            SecurityGroupId = 2
        };

        // Assert
        Assert.That(model.TemplateId, Is.EqualTo(1));
        Assert.That(model.SecurityGroupId, Is.EqualTo(2));
    }

    [Test]
    public void EformInGroup_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new EformInGroup();

        // Assert
        Assert.That(model.EformPermissions, Is.Not.Null);
        Assert.That(model.EformPermissions.Count, Is.EqualTo(0));
    }

    [Test]
    public void EformPermission_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformPermission
        {
            PermissionId = 1,
            EformInGroupId = 2
        };

        // Assert
        Assert.That(model.PermissionId, Is.EqualTo(1));
        Assert.That(model.EformInGroupId, Is.EqualTo(2));
    }

    #endregion

    #region Report Entity Tests

    [Test]
    public void EformReport_CanSetAndGetProperties()
    {
        // Arrange & Act
        var headerImage = new byte[] { 1, 2, 3, 4 };
        var model = new EformReport
        {
            TemplateId = 1,
            Description = "Test Report",
            HeaderImage = headerImage,
            HeaderVisibility = "visible",
            IsDateVisible = true,
            IsWorkerNameVisible = false
        };

        // Assert
        Assert.That(model.TemplateId, Is.EqualTo(1));
        Assert.That(model.Description, Is.EqualTo("Test Report"));
        Assert.That(model.HeaderImage, Is.EqualTo(headerImage));
        Assert.That(model.HeaderVisibility, Is.EqualTo("visible"));
        Assert.That(model.IsDateVisible, Is.True);
        Assert.That(model.IsWorkerNameVisible, Is.False);
    }

    [Test]
    public void EformReport_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new EformReport();

        // Assert
        Assert.That(model.ReportElements, Is.Not.Null);
        Assert.That(model.ReportElements.Count, Is.EqualTo(0));
    }

    [Test]
    public void EformReportElement_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformReportElement
        {
            ElementId = 1,
            EformReportId = 2,
            ParentId = 3
        };

        // Assert
        Assert.That(model.ElementId, Is.EqualTo(1));
        Assert.That(model.EformReportId, Is.EqualTo(2));
        Assert.That(model.ParentId, Is.EqualTo(3));
    }

    [Test]
    public void EformReportElement_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new EformReportElement();

        // Assert
        Assert.That(model.NestedElements, Is.Not.Null);
        Assert.That(model.NestedElements.Count, Is.EqualTo(0));
        Assert.That(model.DataItems, Is.Not.Null);
        Assert.That(model.DataItems.Count, Is.EqualTo(0));
    }

    [Test]
    public void EformReportDataItem_CanSetAndGetProperties()
    {
        // Arrange & Act
        var model = new EformReportDataItem
        {
            DataItemId = 1,
            Position = 2,
            Visibility = true,
            EformReportElementId = 3,
            ParentId = 4
        };

        // Assert
        Assert.That(model.DataItemId, Is.EqualTo(1));
        Assert.That(model.Position, Is.EqualTo(2));
        Assert.That(model.Visibility, Is.True);
        Assert.That(model.EformReportElementId, Is.EqualTo(3));
        Assert.That(model.ParentId, Is.EqualTo(4));
    }

    [Test]
    public void EformReportDataItem_Collections_AreInitialized()
    {
        // Arrange & Act
        var model = new EformReportDataItem();

        // Assert
        Assert.That(model.NestedDataItems, Is.Not.Null);
        Assert.That(model.NestedDataItems.Count, Is.EqualTo(0));
    }

    #endregion
}
