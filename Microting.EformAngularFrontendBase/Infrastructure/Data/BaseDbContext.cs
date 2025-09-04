﻿/*
The MIT License (MIT)

Copyright (c) 2007 - 2021 Microting A/S

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

namespace Microting.EformAngularFrontendBase.Infrastructure.Data
{
    using eFormApi.BasePn.Infrastructure.Database.Entities;
    using eFormApi.BasePn.Infrastructure.Database.Extensions;
    using Entities;
    using Entities.Mailing;
    using Entities.Menu;
    using Entities.Permissions;
    using Entities.Reports;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Seed;

    public class BaseDbContext : IdentityDbContext<EformUser,
        EformRole,
        int,
        IdentityUserClaim<int>,
        EformUserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>>
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options)
        {
        }

        // Common
        public DbSet<SavedTag> SavedTags { get; set; }
        public DbSet<EformConfigurationValue> ConfigurationValues { get; set; }
        public DbSet<EformPlugin> EformPlugins { get; set; }

        // Menu
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuTemplate> MenuTemplates { get; set; }
        public DbSet<MenuItemTranslation> MenuItemTranslations { get; set; }
        public DbSet<MenuTemplateTranslation> MenuTemplateTranslations { get; set; }
        public DbSet<MenuItemSecurityGroup> MenuItemSecurityGroups { get; set; }
        public DbSet<MenuTemplatePermission> MenuTemplatePermissions { get; set; }

        //public DbSet<MenuTranslation> MenuTranslations { get; set; }

        // Reports
        public DbSet<EformReport> EformReports { get; set; }
        public DbSet<EformReportElement> EformReportElements { get; set; }
        public DbSet<EformReportDataItem> EformReportDataItems { get; set; }

        // Security
        public DbSet<SecurityGroup> SecurityGroups { get; set; }
        public DbSet<SecurityGroupUser> SecurityGroupUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<EformInGroup> EformInGroups { get; set; }
        public DbSet<EformPermission> EformPermissions { get; set; }

        // Mailing
        public DbSet<CasePostEmailRecipient> CasePostEmailRecipients { get; set; }
        public DbSet<CasePostEmailTag> CasePostEmailTags { get; set; }
        public DbSet<CasePost> CasePosts { get; set; }
        public DbSet<EmailTagRecipient> EmailTagRecipients { get; set; }
        public DbSet<EmailRecipient> EmailRecipients { get; set; }
        public DbSet<EmailTag> EmailTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EformPlugin>()
                .HasIndex(p => p.PluginId)
                .IsUnique();

            // Menu
            //modelBuilder.Entity<MenuTranslation>()
            //    .HasIndex(p => p.LocaleName);

            modelBuilder.Entity<MenuItemTranslation>()
                .HasIndex(p => p.LocaleName);

            modelBuilder.Entity<MenuTemplateTranslation>()
                .HasIndex(p => p.LocaleName);

            // Reports
            modelBuilder.Entity<EformReport>()
                .HasIndex(p => p.TemplateId)
                .IsUnique();

            modelBuilder.Entity<EformReportElement>()
                .HasIndex(p => p.ElementId);

            modelBuilder.Entity<EformReportElement>()
                .HasMany(e => e.NestedElements)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EformReportDataItem>()
                .HasIndex(p => p.DataItemId);

            modelBuilder.Entity<EformReportDataItem>()
                .HasMany(e => e.NestedDataItems)
                .WithOne(e => e.Parent)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Security
            modelBuilder.Entity<SecurityGroupUser>()
                .HasIndex(p => new
                {
                    p.EformUserId,
                    p.SecurityGroupId
                }).IsUnique();

            modelBuilder.Entity<GroupPermission>()
                .HasIndex(p => new
                {
                    p.PermissionId,
                    p.SecurityGroupId
                }).IsUnique();

            modelBuilder.Entity<PermissionType>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Permission>()
                .HasIndex(p => p.ClaimName)
                .IsUnique();

            modelBuilder.Entity<EformPermission>()
                .HasIndex(p => new
                {
                    p.PermissionId,
                    p.EformInGroupId
                }).IsUnique();

            modelBuilder.Entity<EformInGroup>()
                .HasIndex(p => new
                {
                    p.TemplateId,
                    p.SecurityGroupId
                }).IsUnique();

            modelBuilder.Entity<EformInGroup>()
                .HasIndex(p => p.TemplateId);

            modelBuilder.Entity<EformConfigurationValue>()
                .HasKey(value => value.Id);

            modelBuilder.Entity<EformConfigurationValue>()
                .HasIndex(value => value.Id)
                .IsUnique();

            modelBuilder.Entity<SavedTag>()
                .HasIndex(p => new
                {
                    p.EformUserId,
                    p.TagId
                }).IsUnique();

            // Mailing
            modelBuilder.Entity<EmailTag>()
                .HasIndex(i => i.Name);

            modelBuilder.Entity<EmailRecipient>()
                .HasIndex(i => i.Name);

            modelBuilder.Entity<EmailRecipient>()
                .HasIndex(i => i.Email);

            modelBuilder.Entity<CasePost>()
                .HasIndex(i => i.CaseId);

            modelBuilder.Entity<MenuItemSecurityGroup>()
                .HasIndex(t => new { t.MenuItemId, t.SecurityGroupId })
                .IsUnique();

            modelBuilder.Entity<MenuItemSecurityGroup>()
                .HasOne(sc => sc.MenuItem)
                .WithMany(s => s.MenuItemSecurityGroups)
                .HasForeignKey(sc => sc.MenuItemId);

            modelBuilder.Entity<MenuItemSecurityGroup>()
                .HasOne(sc => sc.SecurityGroup)
                .WithMany(c => c.MenuItemSecurityGroups)
                .HasForeignKey(sc => sc.SecurityGroupId);

            modelBuilder.Entity<MenuTemplatePermission>()
             .HasIndex(t => new { t.MenuTemplateId, t.PermissionId })
             .IsUnique();

            modelBuilder.Entity<MenuTemplatePermission>()
                .HasOne(sc => sc.MenuTemplate)
                .WithMany(s => s.MenuTemplatePermissions)
                .HasForeignKey(sc => sc.MenuTemplateId);

            modelBuilder.Entity<MenuTemplatePermission>()
                .HasOne(sc => sc.Permission)
                .WithMany(c => c.MenuTemplatePermissions)
                .HasForeignKey(sc => sc.PermissionId);


            modelBuilder.Entity<MenuTemplateTranslation>()
                .HasOne<MenuTemplate>(e => e.MenuTemplate)
                .WithMany(d => d.Translations)
                .HasForeignKey(e => e.MenuTemplateId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemTranslation>()
               .HasOne<MenuItem>(e => e.MenuItem)
               .WithMany(d => d.Translations)
               .HasForeignKey(e => e.MenuItemId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItemSecurityGroup>()
                .HasOne<MenuItem>(e => e.MenuItem)
                .WithMany(d => d.MenuItemSecurityGroups)
                .HasForeignKey(e => e.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuTemplatePermission>()
                 .HasOne<Permission>(x => x.Permission)
                 .WithMany(d => d.MenuTemplatePermissions)
                 .HasForeignKey(d => d.PermissionId)
                 .OnDelete(DeleteBehavior.Cascade);

            // if we drop menu template, then dropped all records in table MenuTemplatePermissions
            modelBuilder.Entity<MenuTemplate>()
                .HasMany<MenuTemplatePermission>(x => x.MenuTemplatePermissions)
                .WithOne(x => x.MenuTemplate)
                .HasForeignKey(x => x.MenuTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasOne<MenuTemplate>(e => e.MenuTemplate)
                .WithMany(d => d.MenuItems)
                .HasForeignKey(e => e.MenuTemplateId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // if we drop parent, then dropped all his children
            modelBuilder.Entity<MenuItem>()
              .HasOne<MenuItem>(x => x.Parent)
              .WithMany(d => d.ChildItems)
              .HasForeignKey(d => d.ParentId)
              .OnDelete(DeleteBehavior.Cascade);

            // Extended User configuration for OAuth login support
            modelBuilder.Entity<EformUser>()
                .Property<string>("GoogleId")
                .HasColumnType("longtext")
                .IsRequired(false);

            modelBuilder.Entity<EformUser>()
                .Property<string>("FacebookId")
                .HasColumnType("longtext")
                .IsRequired(false);

            modelBuilder.Entity<EformUser>()
                .Property<string>("GoogleEmail")
                .HasColumnType("longtext")
                .IsRequired(false);

            modelBuilder.Entity<EformUser>()
                .Property<string>("FacebookEmail")
                .HasColumnType("longtext")
                .IsRequired(false);

            modelBuilder.Entity<EformUser>()
                .Property<bool>("ExternalLoginEnabled")
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(true);

            modelBuilder.Entity<EformUser>()
                .Property<string>("PreferredLoginProvider")
                .HasMaxLength(50)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            // Seed
            modelBuilder.SeedLatest();
            // Identity
            modelBuilder.AddIdentityRules();
        }
    }
}