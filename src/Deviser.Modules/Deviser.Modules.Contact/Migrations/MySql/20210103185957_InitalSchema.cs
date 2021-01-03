using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Deviser.Modules.ContactForm.Migrations.MySql
{
    public partial class InitalSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dmc_Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    PageModuleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Data = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dmc_Contact", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dmc_Contact");
        }
    }
}
