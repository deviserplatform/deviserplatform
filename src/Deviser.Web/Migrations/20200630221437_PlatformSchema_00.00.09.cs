using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldDescription",
                table: "ContentTypeField",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "ContentTypeField",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldDescription",
                table: "ContentTypeField");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "ContentTypeField");
        }
    }
}
