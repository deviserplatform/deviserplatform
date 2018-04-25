using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class Platform_Schema_000005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleProperty");

            migrationBuilder.CreateTable(
                name: "ModuleActionProperty",
                columns: table => new
                {
                    ModuleId = table.Column<Guid>(nullable: false),
                    PropertyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleActionProperty", x => new { x.ModuleId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ModuleActionProperty_ModuleAction_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "ModuleAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleActionProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleActionProperty_PropertyId",
                table: "ModuleActionProperty",
                column: "PropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleActionProperty");

            migrationBuilder.CreateTable(
                name: "ModuleProperty",
                columns: table => new
                {
                    ModuleId = table.Column<Guid>(nullable: false),
                    PropertyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleProperty", x => new { x.ModuleId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_ModuleProperty_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleProperty_PropertyId",
                table: "ModuleProperty",
                column: "PropertyId");
        }
    }
}
