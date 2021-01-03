using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Modules.Blog.Migrations.MySql
{
    public partial class InitalSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dm_blog_Blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_Blog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dm_blog_Category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dm_blog_Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dm_blog_Post",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Slug = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Summary = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Thumbnail = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Status = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    IsCommentEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BlogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dm_blog_Post_dm_blog_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "dm_blog_Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dm_blog_Post_dm_blog_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "dm_blog_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dm_blog_Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsApproved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PostId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dm_blog_Comment_dm_blog_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "dm_blog_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dm_blog_PostTag",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TagsId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dm_blog_PostTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_dm_blog_PostTag_dm_blog_Post_PostsId",
                        column: x => x.PostsId,
                        principalTable: "dm_blog_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dm_blog_PostTag_dm_blog_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "dm_blog_Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Blog_Name",
                table: "dm_blog_Blog",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Comment_PostId",
                table: "dm_blog_Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Post_BlogId",
                table: "dm_blog_Post",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Post_CategoryId",
                table: "dm_blog_Post",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Post_Slug",
                table: "dm_blog_Post",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_PostTag_TagsId",
                table: "dm_blog_PostTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_dm_blog_Tag_Name",
                table: "dm_blog_Tag",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dm_blog_Comment");

            migrationBuilder.DropTable(
                name: "dm_blog_PostTag");

            migrationBuilder.DropTable(
                name: "dm_blog_Post");

            migrationBuilder.DropTable(
                name: "dm_blog_Tag");

            migrationBuilder.DropTable(
                name: "dm_blog_Blog");

            migrationBuilder.DropTable(
                name: "dm_blog_Category");
        }
    }
}
