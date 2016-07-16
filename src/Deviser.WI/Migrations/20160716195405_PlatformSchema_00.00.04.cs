using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PropertyOptionList",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PropertyOptionList",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "PropertyOptionList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PropertyOptionList");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PropertyOptionList");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "PropertyOptionList");
        }
    }
}
