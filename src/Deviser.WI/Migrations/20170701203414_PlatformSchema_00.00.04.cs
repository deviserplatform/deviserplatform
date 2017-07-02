using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OptionListId",
                table: "ContentControl",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContentControl_OptionListId",
                table: "ContentControl",
                column: "OptionListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControl_OptionList_OptionListId",
                table: "ContentControl",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentControl_OptionList_OptionListId",
                table: "ContentControl");

            migrationBuilder.DropIndex(
                name: "IX_ContentControl_OptionListId",
                table: "ContentControl");

            migrationBuilder.DropColumn(
                name: "OptionListId",
                table: "ContentControl");
        }
    }
}
