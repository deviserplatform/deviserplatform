using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FieldLabel",
                table: "ContentTypeField",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsList",
                table: "ContentType",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldLabel",
                table: "ContentTypeField");

            migrationBuilder.AlterColumn<string>(
                name: "IsList",
                table: "ContentType",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
