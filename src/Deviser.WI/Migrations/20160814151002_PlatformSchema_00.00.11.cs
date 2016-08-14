using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PageTranslation_Name_Locale",
                table: "PageTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "PageTranslation",
                maxLength: 255,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PageTranslation",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PageTranslation_URL",
                table: "PageTranslation",
                column: "URL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PageTranslation_URL",
                table: "PageTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "PageTranslation",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PageTranslation",
                maxLength: 100,
                nullable: false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PageTranslation_Name_Locale",
                table: "PageTranslation",
                columns: new[] { "Name", "Locale" });
        }
    }
}
