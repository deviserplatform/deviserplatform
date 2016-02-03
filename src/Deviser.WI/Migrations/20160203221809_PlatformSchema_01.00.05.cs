using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_010005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_Role_RoleId", table: "RoleClaim");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_User_UserId", table: "UserClaim");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_User_UserId", table: "UserLogin");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_Role_RoleId", table: "UserRole");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_User_UserId", table: "UserRole");
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "PageModule",
                nullable: false);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_Role_RoleId",
                table: "RoleClaim",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_User_UserId",
                table: "UserClaim",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_User_UserId",
                table: "UserLogin",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_IdentityRoleClaim<string>_Role_RoleId", table: "RoleClaim");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserClaim<string>_User_UserId", table: "UserClaim");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserLogin<string>_User_UserId", table: "UserLogin");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_Role_RoleId", table: "UserRole");
            migrationBuilder.DropForeignKey(name: "FK_IdentityUserRole<string>_User_UserId", table: "UserRole");
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PageModule",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityRoleClaim<string>_Role_RoleId",
                table: "RoleClaim",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserClaim<string>_User_UserId",
                table: "UserClaim",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserLogin<string>_User_UserId",
                table: "UserLogin",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole<string>_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
