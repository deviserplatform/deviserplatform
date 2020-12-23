using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Modules.Blog.Migrations
{
    public partial class BlogSchema_000001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCommentEnabled",
                table: "dm_Post",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommentEnabled",
                table: "dm_Post");
        }
    }
}
