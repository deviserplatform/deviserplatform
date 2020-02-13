using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Deviser.Web.Migrations
{
    public partial class InitialPlatformSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CultureCode = table.Column<string>(nullable: true),
                    EnglishName = table.Column<string>(nullable: true),
                    NativeName = table.Column<string>(nullable: true),
                    FallbackCulture = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Layout",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Config = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LayoutType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    LayoutTypeIds = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Label = table.Column<string>(maxLength: 128, nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Version = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleActionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ControlType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OptionList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    List = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Entity = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SettingName = table.Column<string>(maxLength: 50, nullable: true),
                    SettingValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActionName = table.Column<string>(maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 50, nullable: true),
                    ControllerNamespace = table.Column<string>(maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    ModuleActionTypeId = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleAction_ModuleActionType_ModuleActionTypeId",
                        column: x => x.ModuleActionTypeId,
                        principalTable: "ModuleActionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleAction_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OptionListId = table.Column<Guid>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Property_OptionList_OptionListId",
                        column: x => x.OptionListId,
                        principalTable: "OptionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    IsSystem = table.Column<bool>(nullable: false, defaultValue: false),
                    IsIncludedInMenu = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LayoutId = table.Column<Guid>(nullable: true),
                    PageLevel = table.Column<int>(nullable: true),
                    PageOrder = table.Column<int>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    PageTypeId = table.Column<Guid>(nullable: true),
                    ThemeSrc = table.Column<string>(maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Page_Layout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Page_PageType_PageTypeId",
                        column: x => x.PageTypeId,
                        principalTable: "PageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Page_Page_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        .Annotation("Sqlite:Autoincrement", true)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                        .Annotation("Sqlite:Autoincrement", true)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(nullable: false),
                    ContentTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeProperty", x => new { x.ContentTypeId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ContentTypeProperty_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LayoutTypeProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(nullable: false),
                    LayoutTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutTypeProperty", x => new { x.LayoutTypeId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                        column: x => x.LayoutTypeId,
                        principalTable: "LayoutType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LayoutTypeProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleActionProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(nullable: false),
                    ModuleActionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleActionProperty", x => new { x.ModuleActionId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ModuleActionProperty_ModuleAction_ModuleActionId",
                        column: x => x.ModuleActionId,
                        principalTable: "ModuleAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleActionProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ContainerId = table.Column<Guid>(nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    PageId = table.Column<Guid>(nullable: false),
                    ContentTypeId = table.Column<Guid>(nullable: false),
                    InheritViewPermissions = table.Column<bool>(nullable: false, defaultValue: true),
                    InheritEditPermissions = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageContent_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageContent_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ContainerId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    ModuleActionId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    InheritViewPermissions = table.Column<bool>(nullable: false, defaultValue: true),
                    InheritEditPermissions = table.Column<bool>(nullable: false, defaultValue: true),
                    Properties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageModule_ModuleAction_ModuleActionId",
                        column: x => x.ModuleActionId,
                        principalTable: "ModuleAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageModule_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PageModule_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagePermission_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageTranslation",
                columns: table => new
                {
                    PageId = table.Column<Guid>(nullable: false),
                    Locale = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(maxLength: 500, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    URL = table.Column<string>(maxLength: 255, nullable: true),
                    RedirectUrl = table.Column<string>(nullable: true),
                    IsLinkNewWindow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageTranslation", x => new { x.PageId, x.Locale });
                    table.ForeignKey(
                        name: "FK_PageTranslation_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PageContentId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentPermission_PageContent_PageContentId",
                        column: x => x.PageContentId,
                        principalTable: "PageContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentPermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PageContentTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentData = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CultureCode = table.Column<string>(maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    PageContentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContentTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageContentTranslation_PageContent_PageContentId",
                        column: x => x.PageContentId,
                        principalTable: "PageContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModulePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PageModuleId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulePermission_PageModule_PageModuleId",
                        column: x => x.PageModuleId,
                        principalTable: "PageModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModulePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModulePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentPermission_PageContentId",
                table: "ContentPermission",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPermission_PermissionId",
                table: "ContentPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentPermission_RoleId",
                table: "ContentPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeProperty_PropertyId",
                table: "ContentTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutTypeProperty_PropertyId",
                table: "LayoutTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAction_ModuleActionTypeId",
                table: "ModuleAction",
                column: "ModuleActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_ModuleActions_Modules",
                table: "ModuleAction",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleActionProperty_PropertyId",
                table: "ModuleActionProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermission_PageModuleId",
                table: "ModulePermission",
                column: "PageModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermission_PermissionId",
                table: "ModulePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePermission_RoleId",
                table: "ModulePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_LayoutId",
                table: "Page",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_PageTypeId",
                table: "Page",
                column: "PageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_Pages_Pages",
                table: "Page",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContent_ContentTypeId",
                table: "PageContent",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContent_PageId",
                table: "PageContent",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContentTranslation_PageContentId",
                table: "PageContentTranslation",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PageModule_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_PageModule_Modules",
                table: "PageModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_PageModule_Module",
                table: "PageModule",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_PageId",
                table: "PagePermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_PermissionId",
                table: "PagePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermission_RoleId",
                table: "PagePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_OptionListId",
                table: "Property",
                column: "OptionListId");

            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                    name: "RoleNameIndex",
                    table: "Role",
                    column: "NormalizedName",
                    unique: true,
                    filter: "[NormalizedName] IS NOT NULL");
            }
            else
            {
                migrationBuilder.CreateIndex(
                   name: "RoleNameIndex",
                   table: "Role",
                   column: "NormalizedName",
                   unique: true);
            }



            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.CreateIndex(
                        name: "UserNameIndex",
                        table: "User",
                        column: "NormalizedUserName",
                        unique: true,
                        filter: "[NormalizedUserName] IS NOT NULL"); 
            }
            else
            {
                migrationBuilder.CreateIndex(
                        name: "UserNameIndex",
                        table: "User",
                        column: "NormalizedUserName",
                        unique: true);
            }

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContentPermission");

            migrationBuilder.DropTable(
                name: "ContentTypeProperty");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "LayoutTypeProperty");

            migrationBuilder.DropTable(
                name: "ModuleActionProperty");

            migrationBuilder.DropTable(
                name: "ModulePermission");

            migrationBuilder.DropTable(
                name: "PageContentTranslation");

            migrationBuilder.DropTable(
                name: "PagePermission");

            migrationBuilder.DropTable(
                name: "PageTranslation");

            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "SiteSetting");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "LayoutType");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "PageModule");

            migrationBuilder.DropTable(
                name: "PageContent");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "OptionList");

            migrationBuilder.DropTable(
                name: "ModuleAction");

            migrationBuilder.DropTable(
                name: "ContentType");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "ModuleActionType");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Layout");

            migrationBuilder.DropTable(
                name: "PageType");
        }
    }
}
