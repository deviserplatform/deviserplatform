using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class Platform_Schema_000006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleActionProperty_ModuleAction_ModuleId",
                table: "ModuleActionProperty");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "ModuleActionProperty",
                newName: "ModuleActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleActionProperty_ModuleAction_ModuleActionId",
                table: "ModuleActionProperty",
                column: "ModuleActionId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleActionProperty_ModuleAction_ModuleActionId",
                table: "ModuleActionProperty");

            migrationBuilder.RenameColumn(
                name: "ModuleActionId",
                table: "ModuleActionProperty",
                newName: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleActionProperty_ModuleAction_ModuleId",
                table: "ModuleActionProperty",
                column: "ModuleId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
