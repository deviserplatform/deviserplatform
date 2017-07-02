using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_PageTranslation_PageId",
                table: "PageTranslation");

            migrationBuilder.CreateTable(
                name: "ContentControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IconClass = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Label = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentControl", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentControlId = table.Column<Guid>(nullable: false),
                    ContentTypeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                        column: x => x.ContentControlId,
                        principalTable: "ContentControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ContentControlId",
                table: "ContentTypeControl",
                column: "ContentControlId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ContentTypeId",
                table: "ContentTypeControl",
                column: "ContentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTypeControl");

            migrationBuilder.DropTable(
                name: "ContentControl");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Role");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_PageTranslation_PageId",
                table: "PageTranslation",
                column: "PageId");
        }
    }
}
