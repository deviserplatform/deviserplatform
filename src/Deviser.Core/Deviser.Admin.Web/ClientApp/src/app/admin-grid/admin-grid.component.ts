import { Component, OnInit, TemplateRef, ViewChild, Inject } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { Router, DefaultUrlSerializer, UrlTree } from '@angular/router';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { Pagination } from '../common/domain-types/pagination';
import { ConfirmDialogComponent } from '../common/components/confirm-dialog/confirm-dialog.component';
import { RecordIdPipe } from '../common/pipes/record-id.pipe';
import { Alert, AlertType } from '../common/domain-types/alert';
import { DOCUMENT } from '@angular/common';
import { WINDOW } from '../common/services/window.service';
import { LabelType } from '../common/domain-types/label-type';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { FormResult } from '../common/domain-types/form-result';


@Component({
  selector: 'app-admin-grid',
  templateUrl: './admin-grid.component.html',
  styleUrls: ['./admin-grid.component.scss']
})
export class AdminGridComponent implements OnInit {

  adminConfig: AdminConfig;
  alerts: Alert[];
  entityRecords: any;
  pagination: Pagination;

  @ViewChild(ConfirmDialogComponent)
  private confirmDialogComponent: ConfirmDialogComponent;

  labelType = LabelType;

  constructor(private adminService: AdminService,
    private recordIdPipe: RecordIdPipe,
    private router: Router) {
    this.alerts = [];
  }

  ngOnInit() {
    this.getAdminConfig();
    this.getAllRecords();
  }


  getAdminConfig(): void {
    this.adminService.getAdminConfig()
      .subscribe(adminConfig => this.adminConfig = adminConfig);
  }

  getAllRecords(pagination: Pagination = null): void {
    this.adminService.getAllRecords(pagination)
      .subscribe(entityRecords => this.onGetAllRecords(entityRecords));
  }

  onChangePage(event: any): void {
    if (event.page != null) {
      this.pagination.pageNo = event.page;
    }
    this.getAllRecords(this.pagination);
  }

  onGetAllRecords(entityRecords: any): void {
    this.entityRecords = entityRecords;
    const paging = this.entityRecords.paging;
    this.pagination = new Pagination(paging.pageNo, paging.pageSize, paging.pageCount, paging.totalRecordCount);
  }

  onNewItem(): void {
    this.router.navigateByUrl('detail/');
  }

  openDeleteConfirmationModal(item: any) {
    this.confirmDialogComponent.openModal(item);
  }

  onYesToDelete(item: any): void {
    console.log('confirm');
    const itemId = this.recordIdPipe.transform(item, this.adminConfig.modelConfig.keyField);
    this.adminService.deleteRecord(itemId)
      .subscribe(response => this.onDeleteResponse(response));
  }

  onDeleteResponse(response: any): void {
    if (response) {
      console.log(response);
      this.getAllRecords(this.pagination);
    }
    else {
      let alert: Alert = {
        alterType: AlertType.Error,
        message: "Unable to delete this item, please contact administrator",
        timeout: 5000
      }
      this.alerts.push(alert);
    }
  }

  onNoToDelete(item: any): void {
    console.log('declined');
  }

  onRowAction(actionName: string, item: any) {
    if (actionName && item) {
      this.adminService.executeGridAction(actionName, item)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onActionResult(formValue: FormResult): void {
    if (formValue && formValue.isSucceeded) {
      let alert: Alert = {
        alterType: AlertType.Success,
        message: formValue.successMessage,
        timeout: 5000
      }
      this.alerts.push(alert);
    }
    else {
      let alert: Alert = {
        alterType: AlertType.Error,
        message: formValue.successMessage,
        timeout: 5000
      }
      this.alerts.push(alert);
    }
  }

  getBadge(item: any, field: Field): string {
    if (!field.fieldOption.labelOption) {
      return "";
    }

    if (!field.fieldOption.labelOption.parameters || !field.fieldOption.labelOption.parameters.paramFieldNameCamelCase) {
      if (field.fieldType == FieldType.CheckBox) {
        return item[field.fieldNameCamelCase] ? "badge-primary" : "badge-secondary";
      }
      else {
        return "badge-light";
      }
    }

    return item[field.fieldOption.labelOption.parameters.paramFieldNameCamelCase];
  }
}


class CustomUrlSerializer extends DefaultUrlSerializer {
  parse(url: string): UrlTree {
    return super.parse(url);
  }
}