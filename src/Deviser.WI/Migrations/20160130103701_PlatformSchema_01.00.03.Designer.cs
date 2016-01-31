using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Deviser.Core.Data.Entities;

namespace Deviser.WI.Migrations
{
    [DbContext(typeof(DeviserDBContext))]
    [Migration("20160130103701_PlatformSchema_01.00.03")]
    partial class PlatformSchema_010003
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Deviser.Core.Data.Entities.Layout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Config");

                    b.Property<bool>("IsDeleted")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 2000);

                    b.Property<bool>("IsActive")
                        .HasAnnotation("Relational:DefaultValue", "True")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActionName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ControllerName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("ControllerNamespace")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("DisplayName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("IsDefault")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<int>("ModuleActionTypeId");

                    b.Property<int>("ModuleId");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId")
                        .HasAnnotation("Relational:Name", "IX_FK_ModuleActions_Modules");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleActionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ControlType")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<DateTime?>("EndDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<bool>("IsDeleted")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<bool>("IsSystem")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<int?>("LayoutId");

                    b.Property<int?>("PageLevel");

                    b.Property<int?>("PageOrder");

                    b.Property<int?>("ParentId");

                    b.Property<string>("SkinSrc")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime?>("StartDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasAnnotation("Relational:Name", "IX_FK_Pages_Pages");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("ContainerId");

                    b.Property<string>("ContentData");

                    b.Property<DateTime?>("CreatedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<string>("CultureCode")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<bool>("IsDeleted")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasAnnotation("Relational:ColumnType", "datetime");

                    b.Property<int>("PageId");

                    b.Property<int>("SortOrder");

                    b.Property<string>("TypeInfoProp");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ContainerId");

                    b.Property<bool>("IsDeleted")
                        .HasAnnotation("Relational:DefaultValue", "False")
                        .HasAnnotation("Relational:DefaultValueType", "System.Boolean");

                    b.Property<int>("ModuleId");

                    b.Property<int>("PageId");

                    b.Property<int>("SortOrder");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId")
                        .HasAnnotation("Relational:Name", "IX_FK_PageModule_Modules");

                    b.HasIndex("PageId")
                        .HasAnnotation("Relational:Name", "IX_FK_PageModule_Module");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageTranslation", b =>
                {
                    b.Property<int>("PageId");

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
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Role", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "Role");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.SiteSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SettingName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("SettingValue");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.sysdiagrams", b =>
                {
                    b.Property<int>("diagram_id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("definition")
                        .HasAnnotation("Relational:ColumnType", "varbinary");

                    b.Property<int>("principal_id");

                    b.Property<int?>("version");

                    b.HasKey("diagram_id");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

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
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "User");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "UserRole");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.ModuleAction", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.ModuleActionType")
                        .WithMany()
                        .HasForeignKey("ModuleActionTypeId");

                    b.HasOne("Deviser.Core.Data.Entities.Module")
                        .WithMany()
                        .HasForeignKey("ModuleId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.Page", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Layout")
                        .WithMany()
                        .HasForeignKey("LayoutId");

                    b.HasOne("Deviser.Core.Data.Entities.Page")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageContent", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page")
                        .WithMany()
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageModule", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Module")
                        .WithMany()
                        .HasForeignKey("ModuleId");

                    b.HasOne("Deviser.Core.Data.Entities.Page")
                        .WithMany()
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Deviser.Core.Data.Entities.PageTranslation", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Page")
                        .WithMany()
                        .HasForeignKey("PageId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Deviser.Core.Data.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("Deviser.Core.Data.Entities.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
