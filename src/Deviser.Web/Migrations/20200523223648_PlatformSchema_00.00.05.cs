using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PageContentTranslation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PageContent");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Page");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Layout");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PageModule",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PageContentTranslation",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PageContent",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Layout",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PageModule");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PageContentTranslation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PageContent");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Layout");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PageModule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PageContentTranslation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PageContent",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Page",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Layout",
                nullable: false,
                defaultValue: false);
        }
    }
}
