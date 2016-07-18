using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Deviser.Core.Data.Entities;

namespace Deviser.WI.Migrations
{
    [DbContext(typeof(DeviserDBContext))]
    [Migration("20160717151235_PlatformSchema_00.00.06")]
    partial class PlatformSchema_000006
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentDataType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Label");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ContentDataType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ContentDataTypeId");

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

                    b.HasIndex("ContentDataTypeId");

                    b.ToTable("ContentType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentTypeProperty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ConentTypeId");

                    b.Property<Guid>("PropertyId");

                    b.HasKey("Id");

                    b.HasIndex("ConentTypeId");

                    b.HasIndex("PropertyId");

                    b.ToTable("ContentTypeProperty");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Language", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CultureCode");

                    b.Property<string>("EnglishName");

                    b.Property<string>("FallbackCulture");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime");

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
                        .HasAnnotation("MaxLength", 50);

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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("LayoutTypeId");

                    b.Property<Guid>("PropertyId");

                    b.HasKey("Id");

                    b.HasIndex("LayoutTypeId");

                    b.HasIndex("PropertyId");

                    b.ToTable("LayoutTypeProperty");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.HasKey("Id");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ControllerName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ControllerNamespace")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("DisplayName")
                        .HasAnnotation("MaxLength", 50);

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

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleActionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ControlType")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.ToTable("ModuleActionType");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsIncludedInMenu");

                    b.Property<bool>("IsSystem")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("LayoutId");

                    b.Property<int?>("PageLevel");

                    b.Property<int?>("PageOrder");

                    b.Property<Guid?>("ParentId");

                    b.Property<string>("SkinSrc")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.HasIndex("ParentId")
                        .HasName("IX_FK_Pages_Pages");

                    b.ToTable("Page");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ContainerId");

                    b.Property<Guid>("ContentTypeId");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("PageId");

                    b.Property<string>("Properties");

                    b.Property<int>("SortOrder");

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

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("CultureCode")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("PageContentId");

                    b.HasKey("Id");

                    b.HasIndex("PageContentId");

                    b.ToTable("PageContentTranslation");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ContainerId");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<Guid>("ModuleActionId");

                    b.Property<Guid>("ModuleId");

                    b.Property<Guid>("PageId");

                    b.Property<int>("SortOrder");

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
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Keywords")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Title")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("URL")
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("PageId", "Locale");

                    b.HasIndex("PageId");

                    b.ToTable("PageTranslation");
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

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(true);

                    b.Property<string>("Label");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name");

                    b.Property<Guid?>("PropertyOptionListId");

                    b.HasKey("Id");

                    b.HasIndex("PropertyOptionListId");

                    b.ToTable("Property");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PropertyOptionList", b =>
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

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("PropertyOptionList");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.SiteSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SettingName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("SettingValue");

                    b.HasKey("Id");

                    b.ToTable("SiteSetting");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.sysdiagrams", b =>
                {
                    b.Property<int>("diagram_id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("definition");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("sysname");

                    b.Property<int>("principal_id");

                    b.Property<int?>("version");

                    b.HasKey("diagram_id")
                        .HasName("PK__sysdiagr__C2B05B617A77B34C");

                    b.HasIndex("principal_id", "name")
                        .IsUnique()
                        .HasName("UK_principal_name");

                    b.ToTable("sysdiagrams");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentType", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ContentDataType", "ContentDataType")
                        .WithMany("ContentTypes")
                        .HasForeignKey("ContentDataTypeId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ContentTypeProperty", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ContentType", "ContentType")
                        .WithMany("ContentTypeProperties")
                        .HasForeignKey("ConentTypeId");

                    b.HasOne("Deviser.Core.Data.Entities.Property", "Property")
                        .WithMany("ContentTypeProperties")
                        .HasForeignKey("PropertyId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.LayoutTypeProperty", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.LayoutType", "LayoutType")
                        .WithMany("LayoutTypeProperties")
                        .HasForeignKey("LayoutTypeId");

                    b.HasOne("Deviser.Core.Data.Entities.Property", "Property")
                        .WithMany("LayoutTypeProperties")
                        .HasForeignKey("PropertyId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleActionType", "ModuleActionType")
                        .WithMany("ModuleAction")
                        .HasForeignKey("ModuleActionTypeId");

                    b.HasOne("Deviser.Core.Data.Entities.Module", "Module")
                        .WithMany("ModuleAction")
                        .HasForeignKey("ModuleId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Layout", "Layout")
                        .WithMany("Page")
                        .HasForeignKey("LayoutId");

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Parent")
                        .WithMany("ChildPage")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ContentType", "ContentType")
                        .WithMany("PageContents")
                        .HasForeignKey("ContentTypeId");

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageContent")
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContentTranslation", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.PageContent", "PageContent")
                        .WithMany("PageContentTranslation")
                        .HasForeignKey("PageContentId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleAction", "ModuleAction")
                        .WithMany("PageModules")
                        .HasForeignKey("ModuleActionId");

                    b.HasOne("Deviser.Core.Data.Entities.Module", "Module")
                        .WithMany("PageModule")
                        .HasForeignKey("ModuleId");

                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageModule")
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PagePermission", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PagePermissions")
                        .HasForeignKey("PageId");

                    b.HasOne("Deviser.Core.Data.Entities.Permission", "Permission")
                        .WithMany("PagePermissions")
                        .HasForeignKey("PermissionId");

                    b.HasOne("Deviser.Core.Data.Entities.Role", "Role")
                        .WithMany("PagePermissions")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageTranslation", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page", "Page")
                        .WithMany("PageTranslation")
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Property", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.PropertyOptionList", "PropertyOptionList")
                        .WithMany("Properties")
                        .HasForeignKey("PropertyOptionListId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
