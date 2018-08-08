﻿// <auto-generated />
using System;
using Deviser.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Deviser.WI.Migrations
{
    [DbContext(typeof(DeviserDbContext))]
    [Migration("20180808194350_PlatformShema_00.00.01")]
    partial class PlatformShema_000001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentPermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PageContentId");

                    b.Property<Guid>("PermissionId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("PageContentId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("ContentPermission");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("IconClass");

                    b.Property<string>("IconImage");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name");

                    b.Property<int>("SortOrder");

                    b.HasKey("Id");

                    b.ToTable("ContentType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentTypeProperty", b =>
                {
                    b.Property<Guid>("ContentTypeId");

                    b.Property<Guid>("PropertyId");

                    b.HasKey("ContentTypeId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("ContentTypeProperty");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("CultureCode");

                    b.Property<string>("EnglishName");

                    b.Property<string>("FallbackCulture");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("NativeName");

                    b.HasKey("Id");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Layout", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Config");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Layout");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.LayoutType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("IconClass");

                    b.Property<string>("IconImage");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("LayoutTypeIds");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LayoutType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.LayoutTypeProperty", b =>
                {
                    b.Property<Guid>("LayoutTypeId");

                    b.Property<Guid>("PropertyId");

                    b.HasKey("LayoutTypeId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("LayoutTypeProperty");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description")
                        .HasMaxLength(2000);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionName")
                        .HasMaxLength(50);

                    b.Property<string>("ControllerName")
                        .HasMaxLength(50);

                    b.Property<string>("ControllerNamespace")
                        .HasMaxLength(200);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(50);

                    b.Property<string>("IconClass");

                    b.Property<string>("IconImage");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<Guid>("ModuleActionTypeId");

                    b.Property<Guid>("ModuleId");

                    b.HasKey("Id");

                    b.HasIndex("ModuleActionTypeId");

                    b.HasIndex("ModuleId")
                        .HasName("IX_FK_ModuleActions_Modules");

                    b.ToTable("ModuleAction");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleActionProperty", b =>
                {
                    b.Property<Guid>("ModuleActionId");

                    b.Property<Guid>("PropertyId");

                    b.HasKey("ModuleActionId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("ModuleActionProperty");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleActionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ControlType")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("ModuleActionType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModulePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PageModuleId");

                    b.Property<Guid>("PermissionId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("PageModuleId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("ModulePermission");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.OptionList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("List");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("OptionList");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsIncludedInMenu");

                    b.Property<bool>("IsSystem")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<Guid?>("LayoutId");

                    b.Property<int?>("PageLevel");

                    b.Property<int?>("PageOrder");

                    b.Property<Guid?>("PageTypeId");

                    b.Property<Guid?>("ParentId");

                    b.Property<float>("SiteMapPriority");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("ThemeSrc")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.HasIndex("PageTypeId");

                    b.HasIndex("ParentId")
                        .HasName("IX_FK_Pages_Pages");

                    b.ToTable("Page");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ContainerId");

                    b.Property<Guid>("ContentTypeId");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool>("InheritEditPermissions")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("InheritViewPermissions")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<Guid>("PageId");

                    b.Property<string>("Properties");

                    b.Property<int>("SortOrder");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ContentTypeId");

                    b.HasIndex("PageId");

                    b.ToTable("PageContent");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContentTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContentData");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("CultureCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<Guid>("PageContentId");

                    b.HasKey("Id");

                    b.HasIndex("PageContentId");

                    b.ToTable("PageContentTranslation");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ContainerId");

                    b.Property<bool>("InheritEditPermissions")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("InheritViewPermissions")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<Guid>("ModuleActionId");

                    b.Property<Guid>("ModuleId");

                    b.Property<Guid>("PageId");

                    b.Property<string>("Properties");

                    b.Property<int>("SortOrder");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ModuleActionId");

                    b.HasIndex("ModuleId")
                        .HasName("IX_FK_PageModule_Modules");

                    b.HasIndex("PageId")
                        .HasName("IX_FK_PageModule_Module");

                    b.ToTable("PageModule");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PagePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PageId");

                    b.Property<Guid>("PermissionId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("PageId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("PagePermission");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageTranslation", b =>
                {
                    b.Property<Guid>("PageId");

                    b.Property<string>("Locale")
                        .HasMaxLength(10);

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<bool>("IsLinkNewWindow");

                    b.Property<string>("Keywords")
                        .HasMaxLength(500);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<string>("RedirectUrl");

                    b.Property<string>("Title")
                        .HasMaxLength(200);

                    b.Property<string>("URL")
                        .HasMaxLength(255);

                    b.HasKey("PageId", "Locale");

                    b.ToTable("PageTranslation");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PageType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Entity");

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Property", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DefaultValue");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name");

                    b.Property<Guid?>("OptionListId");

                    b.HasKey("Id");

                    b.HasIndex("OptionListId");

                    b.ToTable("Property");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

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

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.SiteSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SettingName")
                        .HasMaxLength(50);

                    b.Property<string>("SettingValue");

                    b.HasKey("Id");

                    b.ToTable("SiteSetting");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasMaxLength(100);

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

                    b.ToTable("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentPermission", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.PageContent", "PageContent")
                        .WithMany("ContentPermissions")
                        .HasForeignKey("PageContentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Permission", "Permission")
                        .WithMany("ContentPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Role", "Role")
                        .WithMany("ContentPermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentTypeProperty", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ContentType", "ContentType")
                        .WithMany("ContentTypeProperties")
                        .HasForeignKey("ContentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Property", "Property")
                        .WithMany("ContentTypeProperties")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.LayoutTypeProperty", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.LayoutType", "LayoutType")
                        .WithMany("LayoutTypeProperties")
                        .HasForeignKey("LayoutTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Property", "Property")
                        .WithMany("LayoutTypeProperties")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleActionType", "ModuleActionType")
                        .WithMany("ModuleAction")
                        .HasForeignKey("ModuleActionTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Module", "Module")
                        .WithMany("ModuleAction")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleActionProperty", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleAction", "ModuleAction")
                        .WithMany("ModuleActionProperties")
                        .HasForeignKey("ModuleActionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Property", "Property")
                        .WithMany("ModuleActionProperties")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModulePermission", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.PageModule", "PageModule")
                        .WithMany("ModulePermissions")
                        .HasForeignKey("PageModuleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Permission", "Permission")
                        .WithMany("ModulePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Role", "Role")
                        .WithMany("ModulePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Layout", "Layout")
                        .WithMany("Page")
                        .HasForeignKey("LayoutId");

                    b.HasOne("Deviser.Core.Data.Entities.PageType", "PageType")
                        .WithMany("Page")
                        .HasForeignKey("PageTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Parent")
                        .WithMany("ChildPage")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ContentType", "ContentType")
                        .WithMany("PageContents")
                        .HasForeignKey("ContentTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageContent")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContentTranslation", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.PageContent", "PageContent")
                        .WithMany("PageContentTranslation")
                        .HasForeignKey("PageContentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleAction", "ModuleAction")
                        .WithMany("PageModules")
                        .HasForeignKey("ModuleActionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Module", "Module")
                        .WithMany("PageModule")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageModule")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PagePermission", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PagePermissions")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Permission", "Permission")
                        .WithMany("PagePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Deviser.Core.Data.Entities.Role", "Role")
                        .WithMany("PagePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageTranslation", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageTranslation")
                        .HasForeignKey("PageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Property", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.OptionList", "OptionList")
                        .WithMany("Properties")
                        .HasForeignKey("OptionListId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
