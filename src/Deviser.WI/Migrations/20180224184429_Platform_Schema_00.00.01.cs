using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class Platform_Schema_000001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType");

            migrationBuilder.DropTable(
                name: "ContentDataType");

            migrationBuilder.DropIndex(
                name: "IX_ContentType_ContentDataTypeId",
                table: "ContentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentDataType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDataType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentType_ContentDataTypeId",
                table: "ContentType",
                column: "ContentDataTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType",
                column: "ContentDataTypeId",
                principalTable: "ContentDataType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
