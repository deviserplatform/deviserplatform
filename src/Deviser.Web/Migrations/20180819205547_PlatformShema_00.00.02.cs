using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformShema_000002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageHeaderTags",
                table: "PageTranslation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageHeaderTags",
                table: "PageTranslation");
        }
    }
}
