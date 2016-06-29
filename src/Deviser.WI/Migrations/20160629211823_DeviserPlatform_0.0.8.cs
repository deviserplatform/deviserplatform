using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class DeviserPlatform_008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayoutIds",
                table: "LayoutType");

            migrationBuilder.AddColumn<string>(
                name: "LayoutTypeIds",
                table: "LayoutType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LayoutTypeIds",
                table: "LayoutType");

            migrationBuilder.AddColumn<string>(
                name: "LayoutIds",
                table: "LayoutType",
                nullable: true);
        }
    }
}
