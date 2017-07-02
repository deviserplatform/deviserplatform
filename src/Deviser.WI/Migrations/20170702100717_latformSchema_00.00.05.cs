using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class latformSchema_000005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentControl_OptionList_OptionListId",
                table: "ContentControl");

            migrationBuilder.DropIndex(
                name: "IX_ContentControl_OptionListId",
                table: "ContentControl");

            migrationBuilder.AddColumn<Guid>(
                name: "OptionListId",
                table: "ContentTypeControl",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_OptionListId",
                table: "ContentTypeControl",
                column: "OptionListId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl");

            migrationBuilder.DropIndex(
                name: "IX_ContentTypeControl_OptionListId",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "OptionListId",
                table: "ContentTypeControl");

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
    }
}
