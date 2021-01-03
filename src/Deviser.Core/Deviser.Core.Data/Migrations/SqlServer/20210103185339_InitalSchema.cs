using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Core.Data.Migrations.SqlServer
{
    public partial class InitalSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dp_ContentFieldType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ContentFieldType", x => x.Id);
                    table.UniqueConstraint("AK_dp_ContentFieldType_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "dp_ContentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsList = table.Column<bool>(type: "bit", nullable: false),
                    IconImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ContentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FallbackCulture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_Layout",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Config = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Layout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_LayoutType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LayoutTypeIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_LayoutType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_Module",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Label = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_ModuleViewType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControlType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ModuleViewType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_OptionList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    List = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_OptionList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_PageType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_SiteSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SettingName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SettingValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_SiteSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dp_ContentTypeField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FieldDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentFieldTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsShownOnList = table.Column<bool>(type: "bit", nullable: false),
                    IsShownOnPreview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ContentTypeField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_ContentTypeField_dp_ContentFieldType_ContentFieldTypeId",
                        column: x => x.ContentFieldTypeId,
                        principalTable: "dp_ContentFieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ContentTypeField_dp_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "dp_ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_ModuleView",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControllerNamespace = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IconImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleViewTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ModuleView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_ModuleView_dp_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "dp_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ModuleView_dp_ModuleViewType_ModuleViewTypeId",
                        column: x => x.ModuleViewTypeId,
                        principalTable: "dp_ModuleViewType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_Property",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OptionListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_Property_dp_OptionList_OptionListId",
                        column: x => x.OptionListId,
                        principalTable: "dp_OptionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_Page",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsIncludedInMenu = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LayoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageLevel = table.Column<int>(type: "int", nullable: true),
                    PageOrder = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThemeSrc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SiteMapPriority = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_Page_dp_Layout_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "dp_Layout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_Page_dp_Page_ParentId",
                        column: x => x.ParentId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_Page_dp_PageType_PageTypeId",
                        column: x => x.PageTypeId,
                        principalTable: "dp_PageType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_RoleClaim_dp_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "dp_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dp_UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_UserClaim_dp_User_UserId",
                        column: x => x.UserId,
                        principalTable: "dp_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dp_UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_dp_UserLogin_dp_User_UserId",
                        column: x => x.UserId,
                        principalTable: "dp_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dp_UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_dp_UserRole_dp_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "dp_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dp_UserRole_dp_User_UserId",
                        column: x => x.UserId,
                        principalTable: "dp_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dp_UserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_dp_UserToken_dp_User_UserId",
                        column: x => x.UserId,
                        principalTable: "dp_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dp_ContentTypeProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ContentTypeProperty", x => new { x.ContentTypeId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_dp_ContentTypeProperty_dp_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "dp_ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ContentTypeProperty_dp_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "dp_Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_LayoutTypeProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LayoutTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_LayoutTypeProperty", x => new { x.LayoutTypeId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_dp_LayoutTypeProperty_dp_LayoutType_LayoutTypeId",
                        column: x => x.LayoutTypeId,
                        principalTable: "dp_LayoutType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_LayoutTypeProperty_dp_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "dp_Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_ModuleViewProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleViewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ModuleViewProperty", x => new { x.ModuleViewId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_dp_ModuleViewProperty_dp_ModuleView_ModuleViewId",
                        column: x => x.ModuleViewId,
                        principalTable: "dp_ModuleView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ModuleViewProperty_dp_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "dp_Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_AdminPage",
                columns: table => new
                {
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_AdminPage", x => x.PageId);
                    table.ForeignKey(
                        name: "FK_dp_AdminPage_dp_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "dp_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_AdminPage_dp_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_PageContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InheritViewPermissions = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    InheritEditPermissions = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PageContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_PageContent_dp_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "dp_ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_PageContent_dp_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_PageModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleViewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InheritViewPermissions = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    InheritEditPermissions = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PageModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_PageModule_dp_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "dp_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_PageModule_dp_ModuleView_ModuleViewId",
                        column: x => x.ModuleViewId,
                        principalTable: "dp_ModuleView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_PageModule_dp_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_PagePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PagePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_PagePermission_dp_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_PagePermission_dp_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "dp_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_PagePermission_dp_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "dp_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_PageTranslation",
                columns: table => new
                {
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Locale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    URL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLinkNewWindow = table.Column<bool>(type: "bit", nullable: false),
                    PageHeaderTags = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PageTranslation", x => new { x.PageId, x.Locale });
                    table.ForeignKey(
                        name: "FK_dp_PageTranslation_dp_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "dp_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_ContentPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ContentPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_ContentPermission_dp_PageContent_PageContentId",
                        column: x => x.PageContentId,
                        principalTable: "dp_PageContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ContentPermission_dp_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "dp_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ContentPermission_dp_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "dp_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_PageContentTranslation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CultureCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PageContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_PageContentTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_PageContentTranslation_dp_PageContent_PageContentId",
                        column: x => x.PageContentId,
                        principalTable: "dp_PageContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dp_ModulePermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dp_ModulePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dp_ModulePermission_dp_PageModule_PageModuleId",
                        column: x => x.PageModuleId,
                        principalTable: "dp_PageModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ModulePermission_dp_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "dp_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dp_ModulePermission_dp_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "dp_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dp_AdminPage_ModuleId",
                table: "dp_AdminPage",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentPermission_PageContentId",
                table: "dp_ContentPermission",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentPermission_PermissionId",
                table: "dp_ContentPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentPermission_RoleId",
                table: "dp_ContentPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentTypeField_ContentFieldTypeId",
                table: "dp_ContentTypeField",
                column: "ContentFieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentTypeField_ContentTypeId",
                table: "dp_ContentTypeField",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ContentTypeProperty_PropertyId",
                table: "dp_ContentTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_LayoutTypeProperty_PropertyId",
                table: "dp_LayoutTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModulePermission_PageModuleId",
                table: "dp_ModulePermission",
                column: "PageModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModulePermission_PermissionId",
                table: "dp_ModulePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModulePermission_RoleId",
                table: "dp_ModulePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModuleView_ModuleId",
                table: "dp_ModuleView",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModuleView_ModuleViewTypeId",
                table: "dp_ModuleView",
                column: "ModuleViewTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_ModuleViewProperty_PropertyId",
                table: "dp_ModuleViewProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_Page_LayoutId",
                table: "dp_Page",
                column: "LayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_Page_PageTypeId",
                table: "dp_Page",
                column: "PageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_Page_ParentId",
                table: "dp_Page",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageContent_ContentTypeId",
                table: "dp_PageContent",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageContent_PageId",
                table: "dp_PageContent",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageContentTranslation_PageContentId",
                table: "dp_PageContentTranslation",
                column: "PageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageModule_ModuleId",
                table: "dp_PageModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageModule_ModuleViewId",
                table: "dp_PageModule",
                column: "ModuleViewId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PageModule_PageId",
                table: "dp_PageModule",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PagePermission_PageId",
                table: "dp_PagePermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PagePermission_PermissionId",
                table: "dp_PagePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_PagePermission_RoleId",
                table: "dp_PagePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_Property_OptionListId",
                table: "dp_Property",
                column: "OptionListId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "dp_Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_dp_RoleClaim_RoleId",
                table: "dp_RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "dp_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "dp_User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_dp_UserClaim_UserId",
                table: "dp_UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_UserLogin_UserId",
                table: "dp_UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dp_UserRole_RoleId",
                table: "dp_UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dp_AdminPage");

            migrationBuilder.DropTable(
                name: "dp_ContentPermission");

            migrationBuilder.DropTable(
                name: "dp_ContentTypeField");

            migrationBuilder.DropTable(
                name: "dp_ContentTypeProperty");

            migrationBuilder.DropTable(
                name: "dp_Language");

            migrationBuilder.DropTable(
                name: "dp_LayoutTypeProperty");

            migrationBuilder.DropTable(
                name: "dp_ModulePermission");

            migrationBuilder.DropTable(
                name: "dp_ModuleViewProperty");

            migrationBuilder.DropTable(
                name: "dp_PageContentTranslation");

            migrationBuilder.DropTable(
                name: "dp_PagePermission");

            migrationBuilder.DropTable(
                name: "dp_PageTranslation");

            migrationBuilder.DropTable(
                name: "dp_RoleClaim");

            migrationBuilder.DropTable(
                name: "dp_SiteSetting");

            migrationBuilder.DropTable(
                name: "dp_UserClaim");

            migrationBuilder.DropTable(
                name: "dp_UserLogin");

            migrationBuilder.DropTable(
                name: "dp_UserRole");

            migrationBuilder.DropTable(
                name: "dp_UserToken");

            migrationBuilder.DropTable(
                name: "dp_ContentFieldType");

            migrationBuilder.DropTable(
                name: "dp_LayoutType");

            migrationBuilder.DropTable(
                name: "dp_PageModule");

            migrationBuilder.DropTable(
                name: "dp_Property");

            migrationBuilder.DropTable(
                name: "dp_PageContent");

            migrationBuilder.DropTable(
                name: "dp_Permission");

            migrationBuilder.DropTable(
                name: "dp_Role");

            migrationBuilder.DropTable(
                name: "dp_User");

            migrationBuilder.DropTable(
                name: "dp_ModuleView");

            migrationBuilder.DropTable(
                name: "dp_OptionList");

            migrationBuilder.DropTable(
                name: "dp_ContentType");

            migrationBuilder.DropTable(
                name: "dp_Page");

            migrationBuilder.DropTable(
                name: "dp_Module");

            migrationBuilder.DropTable(
                name: "dp_ModuleViewType");

            migrationBuilder.DropTable(
                name: "dp_Layout");

            migrationBuilder.DropTable(
                name: "dp_PageType");
        }
    }
}
