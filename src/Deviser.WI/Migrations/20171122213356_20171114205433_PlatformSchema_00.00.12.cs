using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class _20171114205433_PlatformSchema_000012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentControlProperty");

            migrationBuilder.DropTable(
                name: "ContentTypeControl");

            migrationBuilder.DropTable(
                name: "ContentControl");

            migrationBuilder.DropTable(
                name: "Validator");

            migrationBuilder.DropTable(
                name: "FieldType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    FieldTypeId = table.Column<Guid>(nullable: false),
                    IconClass = table.Column<string>(nullable: true),
                    IconImage = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Label = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentControl_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Validator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FieldTypeId = table.Column<Guid>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    RegExp = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Validator_FieldType_FieldTypeId",
                        column: x => x.FieldTypeId,
                        principalTable: "FieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentControlProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentControlId = table.Column<Guid>(nullable: false),
                    PropertyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentControlProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentControlProperty_ContentControl_ContentControlId",
                        column: x => x.ContentControlId,
                        principalTable: "ContentControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentControlProperty_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentControlId = table.Column<Guid>(nullable: false),
                    ContentTypeId = table.Column<Guid>(nullable: false),
                    IsRequired = table.Column<bool>(nullable: false),
                    Label = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OptionListId = table.Column<Guid>(nullable: false),
                    Placeholder = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    ValidatorId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                        column: x => x.ContentControlId,
                        principalTable: "ContentControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_OptionList_OptionListId",
                        column: x => x.OptionListId,
                        principalTable: "OptionList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeControl_Validator_ValidatorId",
                        column: x => x.ValidatorId,
                        principalTable: "Validator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentControl_FieldTypeId",
                table: "ContentControl",
                column: "FieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentControlProperty_ContentControlId",
                table: "ContentControlProperty",
                column: "ContentControlId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentControlProperty_PropertyId",
                table: "ContentControlProperty",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ContentControlId",
                table: "ContentTypeControl",
                column: "ContentControlId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ContentTypeId",
                table: "ContentTypeControl",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_OptionListId",
                table: "ContentTypeControl",
                column: "OptionListId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ValidatorId",
                table: "ContentTypeControl",
                column: "ValidatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Validator_FieldTypeId",
                table: "Validator",
                column: "FieldTypeId");
        }
    }
}
