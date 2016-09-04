using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PageTranslation_URL",
                table: "PageTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "PageTranslation",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "PageTranslation",
                maxLength: 255,
                nullable: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PageTranslation_URL",
                table: "PageTranslation",
                column: "URL");
        }
    }
}
