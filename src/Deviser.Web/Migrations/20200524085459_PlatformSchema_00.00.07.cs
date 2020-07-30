using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule");

            migrationBuilder.DropTable(
                name: "ModuleActionProperty");

            migrationBuilder.DropTable(
                name: "ModuleAction");

            migrationBuilder.DropTable(
                name: "ModuleActionType");

            migrationBuilder.DropIndex(
                name: "IX_PageModule_ModuleActionId",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "ModuleActionId",
                table: "PageModule");

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleViewId",
                table: "PageModule",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ModuleViewType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ControlType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleViewType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleView",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActionName = table.Column<string>(maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(maxLength: 50, nullable: true),
                    ControllerNamespace = table.Column<string>(maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 50, nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    ModuleViewTypeId = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleView_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleView_ModuleViewType_ModuleViewTypeId",
                        column: x => x.ModuleViewTypeId,
                        principalTable: "ModuleViewType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleViewProperty",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(nullable: false),
                    ModuleViewId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleViewProperty", x => new { x.ModuleViewId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ModuleViewProperty_ModuleView_ModuleViewId",
                        column: x => x.ModuleViewId,
                        principalTable: "ModuleView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleViewProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageModule_ModuleViewId",
                table: "PageModule",
                column: "ModuleViewId");

            migrationBuilder.CreateIndex(
                name: "IX_FK_ModuleViews_Modules",
                table: "ModuleView",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleView_ModuleViewTypeId",
                table: "ModuleView",
                column: "ModuleViewTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleViewProperty_PropertyId",
                table: "ModuleViewProperty",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_ModuleView_ModuleViewId",
                table: "PageModule",
                column: "ModuleViewId",
                principalTable: "ModuleView",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_ModuleView_ModuleViewId",
                table: "PageModule");

            migrationBuilder.DropTable(
                name: "ModuleViewProperty");

            migrationBuilder.DropTable(
                name: "ModuleView");

            migrationBuilder.DropTable(
                name: "ModuleViewType");

            migrationBuilder.DropIndex(
                name: "IX_PageModule_ModuleViewId",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "ModuleViewId",
                table: "PageModule");

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleActionId",
                table: "PageModule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ModuleActionType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ControlType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ControllerNamespace = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModuleActionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "ModuleActionProperty",
                columns: table => new
                {
                    ModuleActionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_PageModule_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
