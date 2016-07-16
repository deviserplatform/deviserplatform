using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "LayoutType",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LayoutType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "LayoutType",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ContentType",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ContentType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "ContentType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "LayoutType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LayoutType");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "LayoutType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ContentType");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ContentType");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "ContentType");
        }
    }
}
