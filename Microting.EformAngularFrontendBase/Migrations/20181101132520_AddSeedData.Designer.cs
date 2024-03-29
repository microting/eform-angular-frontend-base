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
// <auto-generated />

namespace Microting.EformAngularFrontendBase.Migrations
{
    using System;
    using Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(BaseDbContext))]
    [Migration("20181101132520_AddSeedData")]
    partial class AddSeedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.EformInGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("SecurityGroupId");

                    b.Property<int>("TemplateId");

                    b.HasKey("Id");

                    b.HasIndex("SecurityGroupId");

                    b.HasIndex("TemplateId");

                    b.HasIndex("TemplateId", "SecurityGroupId")
                        .IsUnique();

                    b.ToTable("EformInGroups");
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.EformPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EformInGroupId");

                    b.Property<int>("PermissionId");

                    b.HasKey("Id");

                    b.HasIndex("EformInGroupId");

                    b.HasIndex("PermissionId", "EformInGroupId")
                        .IsUnique();

                    b.ToTable("EformPermissions");
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.GroupPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PermissionId");

                    b.Property<int>("SecurityGroupId");

                    b.HasKey("Id");

                    b.HasIndex("SecurityGroupId");

                    b.HasIndex("PermissionId", "SecurityGroupId")
                        .IsUnique();

                    b.ToTable("GroupPermissions");

                    b.HasData(
                        new { Id = 1, PermissionId = 29, SecurityGroupId = 1 },
                        new { Id = 2, PermissionId = 27, SecurityGroupId = 1 },
                        new { Id = 3, PermissionId = 28, SecurityGroupId = 1 },
                        new { Id = 4, PermissionId = 30, SecurityGroupId = 1 },
                        new { Id = 5, PermissionId = 31, SecurityGroupId = 1 },
                        new { Id = 6, PermissionId = 32, SecurityGroupId = 1 },
                        new { Id = 7, PermissionId = 34, SecurityGroupId = 1 },
                        new { Id = 8, PermissionId = 33, SecurityGroupId = 1 },
                        new { Id = 9, PermissionId = 35, SecurityGroupId = 1 },
                        new { Id = 10, PermissionId = 36, SecurityGroupId = 1 },
                        new { Id = 11, PermissionId = 42, SecurityGroupId = 1 },
                        new { Id = 12, PermissionId = 37, SecurityGroupId = 1 },
                        new { Id = 13, PermissionId = 29, SecurityGroupId = 2 },
                        new { Id = 14, PermissionId = 42, SecurityGroupId = 2 },
                        new { Id = 15, PermissionId = 34, SecurityGroupId = 2 },
                        new { Id = 16, PermissionId = 33, SecurityGroupId = 2 },
                        new { Id = 17, PermissionId = 35, SecurityGroupId = 2 },
                        new { Id = 18, PermissionId = 37, SecurityGroupId = 2 }
                    );
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("E2EId");

                    b.Property<string>("Link");

                    b.Property<int>("MenuPosition");

                    b.Property<string>("Name")
                        .HasMaxLength(250);

                    b.Property<int?>("ParentId");

                    b.Property<int>("Position");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new { Id = 1, E2EId = "my-eforms", Link = "/", MenuPosition = 1, Name = "My eForms", Position = 0 },
                        new { Id = 2, E2EId = "device-users", Link = "/simplesites", MenuPosition = 1, Name = "Device Users", Position = 1 },
                        new { Id = 3, E2EId = "advanced", Link = "", MenuPosition = 1, Name = "Advanced", Position = 2 },
                        new { Id = 4, E2EId = "sites", Link = "/advanced/sites", MenuPosition = 1, Name = "Sites", ParentId = 3, Position = 0 },
                        new { Id = 5, E2EId = "workers", Link = "/advanced/workers", MenuPosition = 1, Name = "Workers", ParentId = 3, Position = 1 },
                        new { Id = 6, E2EId = "units", Link = "/advanced/units", MenuPosition = 1, Name = "Units", ParentId = 3, Position = 2 },
                        new { Id = 7, E2EId = "search", Link = "/advanced/entity-search", MenuPosition = 1, Name = "Searchable list", ParentId = 3, Position = 3 },
                        new { Id = 8, E2EId = "selectable-list", Link = "/advanced/entity-select", MenuPosition = 1, Name = "Selectable list", ParentId = 3, Position = 4 },
                        new { Id = 9, E2EId = "application-settings", Link = "/application-settings", MenuPosition = 1, Name = "Application settings", ParentId = 3, Position = 5 },
                        new { Id = 10, E2EId = "sign-out-dropdown", Link = "", MenuPosition = 2, Name = "user", Position = 0 },
                        new { Id = 11, E2EId = "user-management-menu", Link = "/account-management/users", MenuPosition = 2, Name = "User Management", ParentId = 10, Position = 0 },
                        new { Id = 12, E2EId = "settings", Link = "/account-management/settings", MenuPosition = 2, Name = "Settings", ParentId = 10, Position = 1 },
                        new { Id = 13, E2EId = "security", Link = "/security", MenuPosition = 2, Name = "Security", ParentId = 10, Position = 2 },
                        new { Id = 14, E2EId = "change-password", Link = "/account-management/change-password", MenuPosition = 2, Name = "Change password", ParentId = 10, Position = 3 },
                        new { Id = 15, E2EId = "sign-out", Link = "/auth/sign-out", MenuPosition = 2, Name = "Logout", ParentId = 10, Position = 4 }
                    );
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimName")
                        .HasMaxLength(250);

                    b.Property<string>("PermissionName")
                        .HasMaxLength(250);

                    b.Property<int>("PermissionTypeId");

                    b.HasKey("Id");

                    b.HasIndex("ClaimName")
                        .IsUnique()
                        .HasFilter("[ClaimName] IS NOT NULL");

                    b.HasIndex("PermissionTypeId");

                    b.ToTable("Permissions");

                    b.HasData(
                        new { Id = 2, ClaimName = "workers_read", PermissionName = "Read", PermissionTypeId = 1 },
                        new { Id = 1, ClaimName = "workers_create", PermissionName = "Create", PermissionTypeId = 1 },
                        new { Id = 4, ClaimName = "workers_delete", PermissionName = "Delete", PermissionTypeId = 1 },
                        new { Id = 3, ClaimName = "workers_update", PermissionName = "Update", PermissionTypeId = 1 },
                        new { Id = 6, ClaimName = "sites_read", PermissionName = "Read", PermissionTypeId = 2 },
                        new { Id = 8, ClaimName = "sites_delete", PermissionName = "Delete", PermissionTypeId = 2 },
                        new { Id = 7, ClaimName = "sites_update", PermissionName = "Update", PermissionTypeId = 2 },
                        new { Id = 10, ClaimName = "entity_search_read", PermissionName = "Read", PermissionTypeId = 3 },
                        new { Id = 9, ClaimName = "entity_search_create", PermissionName = "Create", PermissionTypeId = 3 },
                        new { Id = 12, ClaimName = "entity_search_delete", PermissionName = "Delete", PermissionTypeId = 3 },
                        new { Id = 11, ClaimName = "entity_search_update", PermissionName = "Update", PermissionTypeId = 3 },
                        new { Id = 14, ClaimName = "entity_select_read", PermissionName = "Read", PermissionTypeId = 4 },
                        new { Id = 13, ClaimName = "entity_select_create", PermissionName = "Create", PermissionTypeId = 4 },
                        new { Id = 16, ClaimName = "entity_select_delete", PermissionName = "Delete", PermissionTypeId = 4 },
                        new { Id = 15, ClaimName = "entity_select_update", PermissionName = "Update", PermissionTypeId = 4 },
                        new { Id = 18, ClaimName = "users_read", PermissionName = "Read", PermissionTypeId = 5 },
                        new { Id = 17, ClaimName = "users_create", PermissionName = "Create", PermissionTypeId = 5 },
                        new { Id = 20, ClaimName = "users_delete", PermissionName = "Delete", PermissionTypeId = 5 },
                        new { Id = 19, ClaimName = "users_update", PermissionName = "Update", PermissionTypeId = 5 },
                        new { Id = 21, ClaimName = "units_read", PermissionName = "Read", PermissionTypeId = 6 },
                        new { Id = 22, ClaimName = "units_update", PermissionName = "Update", PermissionTypeId = 6 },
                        new { Id = 24, ClaimName = "device_users_read", PermissionName = "Read", PermissionTypeId = 7 },
                        new { Id = 23, ClaimName = "device_users_create", PermissionName = "Create", PermissionTypeId = 7 },
                        new { Id = 26, ClaimName = "device_users_delete", PermissionName = "Delete", PermissionTypeId = 7 },
                        new { Id = 25, ClaimName = "device_users_update", PermissionName = "Update", PermissionTypeId = 7 },
                        new { Id = 27, ClaimName = "eforms_create", PermissionName = "Create", PermissionTypeId = 9 },
                        new { Id = 28, ClaimName = "eforms_delete", PermissionName = "Delete", PermissionTypeId = 9 },
                        new { Id = 29, ClaimName = "eforms_read", PermissionName = "Read", PermissionTypeId = 9 },
                        new { Id = 30, ClaimName = "eforms_update_columns", PermissionName = "Update columns", PermissionTypeId = 9 },
                        new { Id = 31, ClaimName = "eforms_download_xml", PermissionName = "Download XML", PermissionTypeId = 9 },
                        new { Id = 32, ClaimName = "eforms_upload_zip", PermissionName = "Upload ZIP", PermissionTypeId = 9 },
                        new { Id = 33, ClaimName = "cases_read", PermissionName = "Cases read", PermissionTypeId = 8 },
                        new { Id = 34, ClaimName = "case_read", PermissionName = "Case read", PermissionTypeId = 8 },
                        new { Id = 35, ClaimName = "case_update", PermissionName = "Case update", PermissionTypeId = 8 },
                        new { Id = 36, ClaimName = "case_delete", PermissionName = "Case delete", PermissionTypeId = 8 },
                        new { Id = 37, ClaimName = "case_get_pdf", PermissionName = "Get PDF", PermissionTypeId = 8 },
                        new { Id = 38, ClaimName = "eforms_pairing_read", PermissionName = "Pairing read", PermissionTypeId = 9 },
                        new { Id = 39, ClaimName = "eforms_pairing_update", PermissionName = "Pairing update", PermissionTypeId = 9 },
                        new { Id = 40, ClaimName = "eforms_read_tags", PermissionName = "Read tags", PermissionTypeId = 9 },
                        new { Id = 41, ClaimName = "eforms_update_tags", PermissionName = "Update tags", PermissionTypeId = 9 },
                        new { Id = 42, ClaimName = "eforms_get_csv", PermissionName = "Get CSV", PermissionTypeId = 9 }
                    );
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.PermissionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("PermissionTypes");

                    b.HasData(
                        new { Id = 1, Name = "Workers" },
                        new { Id = 2, Name = "Sites" },
                        new { Id = 3, Name = "Entity search" },
                        new { Id = 4, Name = "Entity select" },
                        new { Id = 5, Name = "User management" },
                        new { Id = 6, Name = "Units" },
                        new { Id = 7, Name = "Device users" },
                        new { Id = 8, Name = "Cases" },
                        new { Id = 9, Name = "Eforms" }
                    );
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.ToTable("SecurityGroups");

                    b.HasData(
                        new { Id = 1, Name = "eForm admins" },
                        new { Id = 2, Name = "eForm users" }
                    );
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroupUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EformUserId");

                    b.Property<int>("SecurityGroupId");

                    b.HasKey("Id");

                    b.HasIndex("SecurityGroupId");

                    b.HasIndex("EformUserId", "SecurityGroupId")
                        .IsUnique();

                    b.ToTable("SecurityGroupUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("GoogleAuthenticatorSecretKey");

                    b.Property<bool>("IsGoogleAuthenticatorEnabled");

                    b.Property<string>("LastName");

                    b.Property<string>("Locale");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.EformInGroup", b =>
                {
                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroup", "SecurityGroup")
                        .WithMany()
                        .HasForeignKey("SecurityGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.EformPermission", b =>
                {
                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.EformInGroup", "EformInGroup")
                        .WithMany("EformPermissions")
                        .HasForeignKey("EformInGroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.GroupPermission", b =>
                {
                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroup", "SecurityGroup")
                        .WithMany()
                        .HasForeignKey("SecurityGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.MenuItem", b =>
                {
                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.MenuItem", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.Permission", b =>
                {
                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.PermissionType", "PermissionType")
                        .WithMany()
                        .HasForeignKey("PermissionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroupUser", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser", "EformUser")
                        .WithMany()
                        .HasForeignKey("EformUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eFormAPI.Web.Infrastructure.Database.Entities.SecurityGroup", "SecurityGroup")
                        .WithMany("SecurityGroupUsers")
                        .HasForeignKey("SecurityGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUserRole", b =>
                {
                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microting.eFormApi.BasePn.Infrastructure.Database.Entities.EformUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
