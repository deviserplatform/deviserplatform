using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsList",
                table: "ContentType",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentFieldType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentFieldType", x => x.Id);
                    table.UniqueConstraint("AK_ContentFieldType_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypeField",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentTypeId = table.Column<Guid>(nullable: false),
                    ContentFieldTypeId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    IsShownOnList = table.Column<bool>(nullable: false),
                    IsShownOnPreview = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypeField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTypeField_ContentFieldType_ContentFieldTypeId",
                        column: x => x.ContentFieldTypeId,
                        principalTable: "ContentFieldType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContentTypeField_ContentType_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalTable: "ContentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeField_ContentFieldTypeId",
                table: "ContentTypeField",
                column: "ContentFieldTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTypeField_ContentTypeId",
                table: "ContentTypeField",
                column: "ContentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentTypeField");

            migrationBuilder.DropTable(
                name: "ContentFieldType");

            migrationBuilder.DropColumn(
                name: "IsList",
                table: "ContentType");
        }
    }
}
