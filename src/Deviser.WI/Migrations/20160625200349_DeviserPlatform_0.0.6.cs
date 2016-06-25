using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class DeviserPlatform_006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "ModuleAction",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconImage",
                table: "ModuleAction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ContentType",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "ModuleAction");

            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "ModuleAction");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ContentType");
        }
    }
}
