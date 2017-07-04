using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "ContentTypeControl",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "ContentTypeControl",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Properties",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "ContentTypeControl");
        }
    }
}
