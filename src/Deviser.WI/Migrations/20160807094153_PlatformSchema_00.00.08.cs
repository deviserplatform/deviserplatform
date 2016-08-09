using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InheritViewPermissions",
                table: "PageModule",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "InheritViewPermissions",
                table: "PageContent",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InheritViewPermissions",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "InheritViewPermissions",
                table: "PageContent");
        }
    }
}
