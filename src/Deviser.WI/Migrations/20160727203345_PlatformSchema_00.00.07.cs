using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentPermission");

            migrationBuilder.DropTable(
                name: "ModulePermission");
        }
    }
}
