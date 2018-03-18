using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class Platform_Schema_000002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultValue",
                table: "Property",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Property",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValue",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Property");
        }
    }
}
