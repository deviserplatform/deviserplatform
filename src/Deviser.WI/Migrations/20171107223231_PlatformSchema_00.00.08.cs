using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Deviser.WI.Migrations
{
    public partial class PlatformSchema_000008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentControlProperty_ContentControl_ContentControlId",
                table: "ContentControlProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentControlProperty_Property_PropertyId",
                table: "ContentControlProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_PageContent_PageContentId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_Permission_PermissionId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_Role_RoleId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_Property_PropertyId",
                table: "ContentTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                table: "LayoutTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutTypeProperty_Property_PropertyId",
                table: "LayoutTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAction_ModuleActionType_ModuleActionTypeId",
                table: "ModuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAction_Module_ModuleId",
                table: "ModuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_PageModule_PageModuleId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_Permission_PermissionId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_Role_RoleId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContent_Page_PageId",
                table: "PageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContentTranslation_PageContent_PageContentId",
                table: "PageContentTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_Module_ModuleId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_Page_PageId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Page_PageId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Permission_PermissionId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Role_RoleId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PageTranslation_Page_PageId",
                table: "PageTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Validator_FieldType_FieldTypeId",
                table: "Validator");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "User");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Role");

            //migrationBuilder.AddColumn<string>(
            //    name: "Discriminator",
            //    table: "UserRole",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AspNetUserTokens_User_UserId",
            //    table: "AspNetUserTokens",
            //    column: "UserId",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControlProperty_ContentControl_ContentControlId",
                table: "ContentControlProperty",
                column: "ContentControlId",
                principalTable: "ContentControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControlProperty_Property_PropertyId",
                table: "ContentControlProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_PageContent_PageContentId",
                table: "ContentPermission",
                column: "PageContentId",
                principalTable: "PageContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_Permission_PermissionId",
                table: "ContentPermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_Role_RoleId",
                table: "ContentPermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType",
                column: "ContentDataTypeId",
                principalTable: "ContentDataType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                table: "ContentTypeControl",
                column: "ContentControlId",
                principalTable: "ContentControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                table: "ContentTypeControl",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl",
                column: "ValidatorId",
                principalTable: "Validator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty",
                column: "ConentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_Property_PropertyId",
                table: "ContentTypeProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                table: "LayoutTypeProperty",
                column: "LayoutTypeId",
                principalTable: "LayoutType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutTypeProperty_Property_PropertyId",
                table: "LayoutTypeProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAction_ModuleActionType_ModuleActionTypeId",
                table: "ModuleAction",
                column: "ModuleActionTypeId",
                principalTable: "ModuleActionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAction_Module_ModuleId",
                table: "ModuleAction",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_PageModule_PageModuleId",
                table: "ModulePermission",
                column: "PageModuleId",
                principalTable: "PageModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_Permission_PermissionId",
                table: "ModulePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_Role_RoleId",
                table: "ModulePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContent_Page_PageId",
                table: "PageContent",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContentTranslation_PageContent_PageContentId",
                table: "PageContentTranslation",
                column: "PageContentId",
                principalTable: "PageContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_Module_ModuleId",
                table: "PageModule",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_Page_PageId",
                table: "PageModule",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Page_PageId",
                table: "PagePermission",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Permission_PermissionId",
                table: "PagePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Role_RoleId",
                table: "PagePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageTranslation_Page_PageId",
                table: "PageTranslation",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Validator_FieldType_FieldTypeId",
                table: "Validator",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetUserTokens_User_UserId",
            //    table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentControlProperty_ContentControl_ContentControlId",
                table: "ContentControlProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentControlProperty_Property_PropertyId",
                table: "ContentControlProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_PageContent_PageContentId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_Permission_PermissionId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentPermission_Role_RoleId",
                table: "ContentPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentTypeProperty_Property_PropertyId",
                table: "ContentTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                table: "LayoutTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutTypeProperty_Property_PropertyId",
                table: "LayoutTypeProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAction_ModuleActionType_ModuleActionTypeId",
                table: "ModuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAction_Module_ModuleId",
                table: "ModuleAction");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_PageModule_PageModuleId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_Permission_PermissionId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_ModulePermission_Role_RoleId",
                table: "ModulePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContent_Page_PageId",
                table: "PageContent");

            migrationBuilder.DropForeignKey(
                name: "FK_PageContentTranslation_PageContent_PageContentId",
                table: "PageContentTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_Module_ModuleId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PageModule_Page_PageId",
                table: "PageModule");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Page_PageId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Permission_PermissionId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermission_Role_RoleId",
                table: "PagePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_PageTranslation_Page_PageId",
                table: "PageTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_Validator_FieldType_FieldTypeId",
                table: "Validator");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "User");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "Role");

            //migrationBuilder.DropColumn(
            //    name: "Discriminator",
            //    table: "UserRole");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControl_FieldType_FieldTypeId",
                table: "ContentControl",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControlProperty_ContentControl_ContentControlId",
                table: "ContentControlProperty",
                column: "ContentControlId",
                principalTable: "ContentControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentControlProperty_Property_PropertyId",
                table: "ContentControlProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_PageContent_PageContentId",
                table: "ContentPermission",
                column: "PageContentId",
                principalTable: "PageContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_Permission_PermissionId",
                table: "ContentPermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentPermission_Role_RoleId",
                table: "ContentPermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentType_ContentDataType_ContentDataTypeId",
                table: "ContentType",
                column: "ContentDataTypeId",
                principalTable: "ContentDataType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_ContentControl_ContentControlId",
                table: "ContentTypeControl",
                column: "ContentControlId",
                principalTable: "ContentControl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_ContentType_ContentTypeId",
                table: "ContentTypeControl",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_OptionList_OptionListId",
                table: "ContentTypeControl",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeControl_Validator_ValidatorId",
                table: "ContentTypeControl",
                column: "ValidatorId",
                principalTable: "Validator",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_ContentType_ConentTypeId",
                table: "ContentTypeProperty",
                column: "ConentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentTypeProperty_Property_PropertyId",
                table: "ContentTypeProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutTypeProperty_LayoutType_LayoutTypeId",
                table: "LayoutTypeProperty",
                column: "LayoutTypeId",
                principalTable: "LayoutType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutTypeProperty_Property_PropertyId",
                table: "LayoutTypeProperty",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAction_ModuleActionType_ModuleActionTypeId",
                table: "ModuleAction",
                column: "ModuleActionTypeId",
                principalTable: "ModuleActionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAction_Module_ModuleId",
                table: "ModuleAction",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_PageModule_PageModuleId",
                table: "ModulePermission",
                column: "PageModuleId",
                principalTable: "PageModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_Permission_PermissionId",
                table: "ModulePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModulePermission_Role_RoleId",
                table: "ModulePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContent_ContentType_ContentTypeId",
                table: "PageContent",
                column: "ContentTypeId",
                principalTable: "ContentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContent_Page_PageId",
                table: "PageContent",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageContentTranslation_PageContent_PageContentId",
                table: "PageContentTranslation",
                column: "PageContentId",
                principalTable: "PageContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_ModuleAction_ModuleActionId",
                table: "PageModule",
                column: "ModuleActionId",
                principalTable: "ModuleAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_Module_ModuleId",
                table: "PageModule",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageModule_Page_PageId",
                table: "PageModule",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Page_PageId",
                table: "PagePermission",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Permission_PermissionId",
                table: "PagePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermission_Role_RoleId",
                table: "PagePermission",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PageTranslation_Page_PageId",
                table: "PageTranslation",
                column: "PageId",
                principalTable: "Page",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_OptionList_OptionListId",
                table: "Property",
                column: "OptionListId",
                principalTable: "OptionList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Validator_FieldType_FieldTypeId",
                table: "Validator",
                column: "FieldTypeId",
                principalTable: "FieldType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
