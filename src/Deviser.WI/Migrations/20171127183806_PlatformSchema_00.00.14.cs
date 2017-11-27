using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutTypeProperty",
                table: "LayoutTypeProperty");

            migrationBuilder.DropIndex(
                name: "IX_LayoutTypeProperty_LayoutTypeId",
                table: "LayoutTypeProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTypeProperty",
                table: "ContentTypeProperty");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ContentTypeProperty_ConentTypeId_PropertyId",
                table: "ContentTypeProperty");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LayoutTypeProperty");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ContentTypeProperty");

            migrationBuilder.RenameColumn(
                name: "ConentTypeId",
                table: "ContentTypeProperty",
                newName: "ContentTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutTypeProperty",
                table: "LayoutTypeProperty",
                columns: new[] { "LayoutTypeId", "PropertyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTypeProperty",
                table: "ContentTypeProperty",
                columns: new[] { "ContentTypeId", "PropertyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ContentTypeId",
                table: "ContentTypeProperty",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ContentTypeId",
                table: "ContentTypeProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutTypeProperty",
                table: "LayoutTypeProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContentTypeProperty",
                table: "ContentTypeProperty");
            
            migrationBuilder.RenameColumn(
               name: "ContentTypeId",
               table: "ContentTypeProperty",
               newName: "ConentTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "LayoutTypeProperty",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ContentTypeProperty",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutTypeProperty",
                table: "LayoutTypeProperty",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContentTypeProperty",
                table: "ContentTypeProperty",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ContentTypeProperty_ConentTypeId_PropertyId",
                table: "ContentTypeProperty",
                columns: new[] { "ConentTypeId", "PropertyId" });

            migrationBuilder.CreateIndex(
                name: "IX_LayoutTypeProperty_LayoutTypeId",
                table: "LayoutTypeProperty",
                column: "LayoutTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty",
                column: "ConentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
