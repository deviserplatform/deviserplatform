using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class DeviserPlatform_007 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PageModule_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule");

            migrationBuilder.DropIndex(
                name: "IX_PageModule_ModuleActionId",
                table: "PageModule");
        }
    }
}
