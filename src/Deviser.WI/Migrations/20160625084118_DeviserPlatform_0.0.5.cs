using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class DeviserPlatform_005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeInfo",
                table: "PageContent");

            migrationBuilder.AddColumn<Guid>(
                name: "ContentTypeId",
                table: "PageContent",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "PageContent",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageContent_ContentTypeId",
                table: "PageContent",
                column: "ContentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent");

            migrationBuilder.DropIndex(
                name: "IX_PageContent_ContentTypeId",
                table: "PageContent");

            migrationBuilder.DropColumn(
                name: "ContentTypeId",
                table: "PageContent");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "PageContent");

            migrationBuilder.AddColumn<string>(
                name: "TypeInfo",
                table: "PageContent",
                nullable: true);
        }
    }
}
