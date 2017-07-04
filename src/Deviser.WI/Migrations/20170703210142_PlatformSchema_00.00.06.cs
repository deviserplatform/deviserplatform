using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionListId",
                table: "ContentControl");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OptionList",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "ContentTypeControl",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "ContentTypeControl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ContentTypeControl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "ContentTypeControl",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ValidatorId",
                table: "ContentTypeControl",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContentDataType",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FieldTypeId",
                table: "ContentControl",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeControl_ValidatorId",
                table: "ContentTypeControl",
                column: "ValidatorId");

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
                name: "IX_Validator_FieldTypeId",
                table: "Validator",
                column: "FieldTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl",
                column: "ValidatorId",
                principalTable: "Validator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl");

            migrationBuilder.DropTable(
                name: "ContentControlProperty");

            migrationBuilder.DropTable(
                name: "Validator");

            migrationBuilder.DropTable(
                name: "FieldType");

            migrationBuilder.DropIndex(
                name: "IX_ContentTypeControl_ValidatorId",
                table: "ContentTypeControl");

            migrationBuilder.DropIndex(
                name: "IX_ContentControl_FieldTypeId",
                table: "ContentControl");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "ValidatorId",
                table: "ContentTypeControl");

            migrationBuilder.DropColumn(
                name: "FieldTypeId",
                table: "ContentControl");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OptionList",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ContentDataType",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "OptionListId",
                table: "ContentControl",
                nullable: true);
        }
    }
}
