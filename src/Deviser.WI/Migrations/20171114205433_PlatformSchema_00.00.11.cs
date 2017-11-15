using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkinSrc",
                table: "Page");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PageModule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PageContent",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThemeSrc",
                table: "Page",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PageContent");

            migrationBuilder.DropColumn(
                name: "ThemeSrc",
                table: "Page");

            migrationBuilder.AddColumn<string>(
                name: "SkinSrc",
                table: "Page",
                maxLength: 200,
                nullable: true);
        }
    }
}
