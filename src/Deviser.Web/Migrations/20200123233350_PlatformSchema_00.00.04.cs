using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Web.Migrations
{
    public partial class PlatformSchema_000004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityName",
                table: "AdminPage",
                newName: "ModelName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "AdminPage",
                newName: "EntityName");
        }
    }
}
