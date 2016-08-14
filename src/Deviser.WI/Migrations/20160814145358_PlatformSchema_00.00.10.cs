using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PageTranslation_Name_Locale",
                table: "PageTranslation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PageTranslation",
                maxLength: 100,
                nullable: true);
        }
    }
}
