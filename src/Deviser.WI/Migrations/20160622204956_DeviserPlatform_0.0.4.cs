using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class DeviserPlatform_004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentDataType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentDataType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LayoutType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IconClass = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    LayoutIds = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyOptionList",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    List = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyOptionList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentDataTypeId = table.Column<Guid>(nullable: false),
                    IconClass = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                        column: x => x.ContentDataTypeId,
                        principalTable: "ContentDataType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PropertyOptionListId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Property_PropertyOptionList_PropertyOptionListId",
                        column: x => x.PropertyOptionListId,
                        principalTable: "PropertyOptionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConentTypeId = table.Column<Guid>(nullable: false),
                    PropertyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                        column: x => x.ConentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LayoutTypeProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LayoutTypeId = table.Column<Guid>(nullable: false),
                    PropertyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LayoutTypeProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                        column: x => x.LayoutTypeId,
                        principalTable: "LayoutType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LayoutTypeProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentType_ContentDataTypeId",
                table: "ContentType",
                column: "ContentDataTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeProperty_ConentTypeId",
                table: "ContentTypeProperty",
                column: "ConentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeProperty_PropertyId",
                table: "ContentTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutTypeProperty_LayoutTypeId",
                table: "LayoutTypeProperty",
                column: "LayoutTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LayoutTypeProperty_PropertyId",
                table: "LayoutTypeProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyOptionListId",
                table: "Property",
                column: "PropertyOptionListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTypeProperty");

            migrationBuilder.DropTable(
                name: "LayoutTypeProperty");

            migrationBuilder.DropTable(
                name: "ContentType");

            migrationBuilder.DropTable(
                name: "LayoutType");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "ContentDataType");

            migrationBuilder.DropTable(
                name: "PropertyOptionList");
        }
    }
}
