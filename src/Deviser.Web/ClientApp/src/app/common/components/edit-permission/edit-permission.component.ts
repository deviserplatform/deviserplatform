import { Component, OnInit, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AlertType, Globals, PageContent, PageModule, PlaceHolder, Role } from 'deviser-shared';
import { AlertService, PageContentService, PageModuleService, RoleService, } from 'deviser-shared';

@Component({
  selector: 'app-edit-permission',
  templateUrl: './edit-permission.component.html',
  styleUrls: ['./edit-permission.component.scss']
})
export class EditPermissionComponent implements OnInit {
  appSettings = Globals.appSettings;
  permissionSaved: EventEmitter<any> = new EventEmitter();

  label: any = {
    title: '',
    tableRowTitleView: '',
    tableRowTitleEdit: ''
  }
  roles: Role[];
  inheritViewPermissions: boolean;
  inheritEditPermissions: boolean;

  get selectedNode(): PlaceHolder {
    return this._selectedNode;
  }

  set selectedNode(value: PlaceHolder) {
    this._selectedNode = value;
    this.initPermission();
  }

  get isPageContent(): boolean {
    return this.selectedNode.pageContent ? true : false;
  }

  private _selectedNode: PlaceHolder;

  constructor(public bsModalRef: BsModalRef,
    private _alertService: AlertService,
    private _pageContentService: PageContentService,
    private _pageModuleService: PageModuleService,
    private _roleService: RoleService) { }

  ngOnInit(): void {
  }

  isViewChecked(role) {
    if (this.isPageContent) {
      //PageContent
      let permission = this.selectedNode.pageContent.contentPermissions.find(p => {
        return p.roleId === role.id && p.permissionId === Globals.appSettings.permissions.contentView;
      });
      return permission;
    } else {
      //PageModule
      let permission = this.selectedNode.pageModule.modulePermissions.find(p => {
        return p.roleId === role.id && p.permissionId === Globals.appSettings.permissions.moduleView;
      });
      return permission;
    }
  }

  isEditChecked(role) {
    if (this.isPageContent) {
      let permission = this.selectedNode.pageContent.contentPermissions.find(p => {
        return p.roleId === role.id && p.permissionId === Globals.appSettings.permissions.contentEdit;
      });
      return permission;
    } else {
      let permission = this.selectedNode.pageModule.modulePermissions.find(p => {
        return p.roleId === role.id && p.permissionId === Globals.appSettings.permissions.moduleEdit;
      });
      return permission;
    }
  }

  changeViewPermission(role: Role) {
    if (role.id === Globals.appSettings.roles.administrator) {
      return;
    }
    let targetPermission = this.isPageContent ? this.appSettings.permissions.contentView : this.appSettings.permissions.moduleView;
    if (this.isViewChecked(role)) {
      //Remove
      if (this.isPageContent) {
        this.selectedNode.pageContent.contentPermissions = this.selectedNode.pageContent.contentPermissions.filter(
          mp => mp.roleId !== role.id || mp.permissionId !== targetPermission);
      }
      else {
        this.selectedNode.pageModule.modulePermissions = this.selectedNode.pageModule.modulePermissions.filter(
          mp => mp.roleId !== role.id || mp.permissionId !== targetPermission);
      }
      //_.reject(vm.pageModule.modulePermissions, function (p) {
      //        return p.roleId === role.id && p.permissionId === moduleViewPermissionId
    }
    else {
      //Add
      if (this.isPageContent) {
        let permission = {
          pageContentId: this.selectedNode.pageContent.id,
          roleId: role.id,
          permissionId: targetPermission
        };
        this.selectedNode.pageContent.contentPermissions.push(permission);
      }
      else {
        let permission = {
          pageModuleId: this.selectedNode.pageModule.id,
          roleId: role.id,
          permissionId: targetPermission
        };
        this.selectedNode.pageModule.modulePermissions.push(permission);
      }

    }

  }

  changeEditPermission(role: Role) {
    if (role.id === Globals.appSettings.roles.administrator) {
      return;
    }

    let targetPermission = this.isPageContent ? this.appSettings.permissions.contentEdit : this.appSettings.permissions.moduleEdit;
    if (this.isEditChecked(role)) {
      //Remove
      if (this.isPageContent) {
        this.selectedNode.pageContent.contentPermissions = this.selectedNode.pageContent.contentPermissions.filter(
          mp => mp.roleId !== role.id || mp.permissionId !== targetPermission);
      }
      else {
        this.selectedNode.pageModule.modulePermissions = this.selectedNode.pageModule.modulePermissions.filter(
          mp => mp.roleId !== role.id || mp.permissionId !== targetPermission);
      }
    }
    else {
      //Add

      if (this.isPageContent) {
        let permission = {
          pageContentId: this.selectedNode.pageContent.id,
          roleId: role.id,
          permissionId: targetPermission
        };
        this.selectedNode.pageContent.contentPermissions.push(permission);
      }
      else {
        let permission = {
          pageModuleId: this.selectedNode.pageModule.id,
          roleId: role.id,
          permissionId: targetPermission
        };
        this.selectedNode.pageModule.modulePermissions.push(permission);
      }
    }

  }

  save() {
    if (this.isPageContent) {
      let pageContent: PageContent = {
        id: this.selectedNode.pageContent.id,
        pageId: this.selectedNode.pageContent.pageId,
        contentTypeId: this.selectedNode.pageContent.contentTypeId,
        inheritViewPermissions: this.inheritViewPermissions,
        inheritEditPermissions: this.inheritEditPermissions,
        contentPermissions: this.selectedNode.pageContent.contentPermissions
      }
      this._pageContentService.updatePageContentPermissions(pageContent).subscribe(pageContent => {
        this._alertService.showMessage(AlertType.Success, "Permissions have been updated");
        let selectedNode: PlaceHolder = JSON.parse(JSON.stringify(this.selectedNode));
        selectedNode.pageContent = pageContent;
        this.permissionSaved.emit(this.selectedNode);
        this.bsModalRef.hide();
      }, error => {
        this._alertService.showMessage(AlertType.Success, "Unable to update permissions");
      });
    } else {
      let pageModule: PageModule = {
        id: this.selectedNode.pageModule.id,
        pageId: this.selectedNode.pageModule.pageId,
        moduleId: this.selectedNode.pageModule.moduleId,
        moduleViewId: this.selectedNode.pageModule.moduleViewId,
        inheritViewPermissions: this.inheritViewPermissions,
        inheritEditPermissions: this.inheritEditPermissions,
        modulePermissions: this.selectedNode.pageModule.modulePermissions
      }
      this._pageModuleService.updatePageModulePermissions(pageModule).subscribe(pageModule => {
        this._alertService.showMessage(AlertType.Success, "Permissions have been updated");
        let selectedNode: PlaceHolder = JSON.parse(JSON.stringify(this.selectedNode));
        selectedNode.pageModule = pageModule;
        this.permissionSaved.emit(this.selectedNode);
        this.bsModalRef.hide();
      }, error => {
        this._alertService.showMessage(AlertType.Success, "Unable to update permissions");
      });
    }
  }

  cancel() {
    this.bsModalRef.hide();
  }

  private initPermission() {
    if (this.selectedNode.pageContent) {
      this.label.title = 'Content Permissions';
      this.label.tableRowTitleEdit = 'Edit Content'
      this.label.tableRowTitleView = 'View Content'
      this.inheritEditPermissions = this.selectedNode.pageContent.inheritEditPermissions;
      this.inheritViewPermissions = this.selectedNode.pageContent.inheritViewPermissions;
    }
    else {
      this.label.title = 'Module Permissions';
      this.label.tableRowTitleEdit = 'Edit Module'
      this.label.tableRowTitleView = 'View Module'
      this.inheritEditPermissions = this.selectedNode.pageModule.inheritEditPermissions;
      this.inheritViewPermissions = this.selectedNode.pageModule.inheritViewPermissions;
    }

    this._roleService.getRoles().subscribe(roles => this.roles = roles,
      error => this._alertService.showMessage(AlertType.Error, "Unable to get the user roles"));
  }

}
