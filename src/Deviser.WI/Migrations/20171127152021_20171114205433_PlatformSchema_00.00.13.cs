using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class _20171114205433_PlatformSchema_000013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContentTypeProperty_ConentTypeId",
                table: "ContentTypeProperty");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ContentTypeProperty_ConentTypeId_PropertyId",
                table: "ContentTypeProperty",
                columns: new[] { "ConentTypeId", "PropertyId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_ContentTypeProperty_ConentTypeId_PropertyId",
                table: "ContentTypeProperty");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeProperty_ConentTypeId",
                table: "ContentTypeProperty",
                column: "ConentTypeId");
        }
    }
}
