using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_PropertyOptionList_PropertyOptionListId",
                table: "Property");

            migrationBuilder.DropTable(
                name: "PropertyOptionList");

            migrationBuilder.RenameColumn(
                name: "PropertyOptionListId",
                table: "Property",
                newName: "OptionListId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_PropertyOptionListId",
                table: "Property",
                newName: "IX_Property_OptionListId");

            migrationBuilder.CreateTable(
                name: "OptionList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Label = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    List = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionList", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property");

            migrationBuilder.DropTable(
                name: "OptionList");

            migrationBuilder.RenameColumn(
                name: "OptionListId",
                table: "Property",
                newName: "PropertyOptionListId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_OptionListId",
                table: "Property",
                newName: "IX_Property_PropertyOptionListId");

            migrationBuilder.CreateTable(
                name: "PropertyOptionList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Label = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    List = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyOptionList", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Property_PropertyOptionList_PropertyOptionListId",
                table: "Property",
                column: "PropertyOptionListId",
                principalTable: "PropertyOptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
