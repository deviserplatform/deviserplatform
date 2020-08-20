import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Router, DefaultUrlSerializer, UrlTree } from '@angular/router';

import { AlertType } from 'deviser-shared';
import { AlertService } from 'deviser-shared';
import { ConfirmDialogComponent } from 'deviser-shared';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { Pagination } from '../common/domain-types/pagination';

import { RecordIdPipe } from '../common/pipes/record-id.pipe';
import { WINDOW } from '../common/services/window.service';
import { LabelType } from '../common/domain-types/label-type';
import { AdminResult } from '../common/domain-types/admin-result';
import { DAConfig } from '../common/domain-types/da-config';
import { AdminConfigType } from '../common/domain-types/admin-confit-type';
import { SortField } from '../common/domain-types/sort-field';
import { SortState } from '../common/domain-types/sort-state';
import { FilterField } from '../common/domain-types/filter-field';
import { FilterNode } from '../common/domain-types/filter-node';
import { LogicalOperator } from '../common/domain-types/logical-operator';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';


@Component({
  selector: 'app-admin-grid',
  templateUrl: './admin-grid.component.html',
  styleUrls: ['./admin-grid.component.scss']
})
export class AdminGridComponent implements OnInit {

  adminConfig: AdminConfig;
  adminConfigType = AdminConfigType;
  entityRecords: any;
  daConfig: DAConfig;
  pagination: Pagination;
  labelType = LabelType;

  @ViewChild(ConfirmDialogComponent)
  private confirmDialogComponent: ConfirmDialogComponent;

  get isSortable(): boolean {
    return this.adminConfig && this.adminConfig.modelConfig &&
      this.adminConfig.modelConfig.gridConfig &&
      this.adminConfig.modelConfig.gridConfig.isSortable;
  }

  constructor(private _adminService: AdminService,
    private _alertService: AlertService,
    private _recordIdPipe: RecordIdPipe,
    private _router: Router,
    @Inject(WINDOW) _window: any) {
    this.getAdminConfig();
    this.daConfig = _window.daConfig;
  }

  ngOnInit() {
    this.getAllRecords();
  }


  getAdminConfig(): void {
    this._adminService.getAdminConfig()
      .subscribe(adminConfig => this.adminConfig = adminConfig);
  }

  getAllRecords(pagination: Pagination = null): void {
    this._adminService.getAllRecords(pagination)
      .subscribe(entityRecords => this.onGetAllRecords(entityRecords));
  }

  drop(event: CdkDragDrop<any[]>) {
    let gridItems = event.container.data;
    moveItemInArray(gridItems, event.previousIndex, event.currentIndex);
    //update sorting
    this.adminConfig.modelConfig
    gridItems.forEach((item, index) => item[this.adminConfig.modelConfig.gridConfig.sortField.fieldNameCamelCase] = index + 1);

    this._adminService.sortGridItems(gridItems).subscribe(result => console.log(result), error => console.log(error));
  }

  onChangePage(event: any): void {
    if (event.page != null) {
      this.pagination.pageNo = event.page;
    }
    this.getAllRecords(this.pagination);
  }

  onGetAllRecords(entityRecords: any): void {
    this.entityRecords = entityRecords;
    let gridConfig = this.adminConfig.modelConfig.gridConfig;
    if (this.isSortable) {
      let sortField = gridConfig.sortField.fieldNameCamelCase;
      this.entityRecords.data = this.entityRecords.data.sort((a, b) => a[sortField] > b[sortField] ? 1 : -1);
    }

    const paging = this.entityRecords.paging;
    this.pagination = new Pagination(paging.pageNo, paging.pageSize, paging.pageCount, paging.totalRecordCount);
  }

  onNewItem(): void {
    this._router.navigateByUrl('detail/');
  }

  openDeleteConfirmationModal(item: any) {
    this.confirmDialogComponent.openModal(item);
  }

  onYesToDelete(item: any): void {
    console.log('confirm');
    const itemId = this._recordIdPipe.transform(item, this.adminConfig.modelConfig.keyField);
    this._adminService.deleteRecord(itemId)
      .subscribe(response => this.onActionResult(response));
  }

  onNoToDelete(): void {
    console.log('declined');
  }

  onRowAction(actionName: string, item: any) {
    if (actionName && item) {
      this._adminService.executeGridAction(actionName, item)
        .subscribe(adminResult => this.onActionResult(adminResult));
    }
  }

  onActionResult(adminResult: AdminResult): void {
    if (adminResult && adminResult.isSucceeded) {
      this._alertService.showMessage(AlertType.Success, adminResult.successMessage);
      this.getAllRecords(this.pagination);
    }
    else {
      this._alertService.showMessage(AlertType.Error, adminResult.errorMessage);
    }
  }

  onFilterSortChange($event): void {
    console.log($event);
    const filterFields: FilterField[] = $event.filters as FilterField[];
    const sortField: SortField = $event.sortField as SortField;
    let orderBy;
    if (sortField && sortField.sortState && sortField.field) {
      orderBy = sortField.sortState === SortState.Descending ? `-${sortField.field.fieldName}` : sortField.field.fieldName;
    }
    const filterNode: FilterNode = {
      rootOperator: LogicalOperator.AND,
      childOperator: LogicalOperator.AND
    };

    filterNode.childNodes = filterFields.map(ff => {
      return {
        rootOperator: LogicalOperator.AND,
        childOperator: LogicalOperator.AND,
        filter: ff.filter
      };
    });

    this._adminService.filterRecords(filterNode, orderBy, this.pagination)
      .subscribe(entityRecords => this.onGetAllRecords(entityRecords));
  }
}

