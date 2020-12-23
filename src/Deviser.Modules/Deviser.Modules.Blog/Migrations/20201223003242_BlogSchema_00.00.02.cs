using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Modules.Blog.Migrations
{
    public partial class BlogSchema_000002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_dm_Tag_Name",
                table: "dm_Tag");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "dm_Tag",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_dm_Tag_Name",
                table: "dm_Tag",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_dm_Tag_Name",
                table: "dm_Tag");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "dm_Tag",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_dm_Tag_Name",
                table: "dm_Tag",
                column: "Name");
        }
    }
}
