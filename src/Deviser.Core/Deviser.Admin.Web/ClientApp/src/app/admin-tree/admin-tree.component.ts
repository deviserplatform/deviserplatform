import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { Router } from '@angular/router';

import { AlertType } from 'deviser-shared';
import { AlertService } from 'deviser-shared';
import { ConfirmDialogComponent } from 'deviser-shared';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { RecordIdPipe } from '../common/pipes/record-id.pipe';
import { WINDOW } from '../common/services/window.service';
import { LabelType } from '../common/domain-types/label-type';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { FormResult } from '../common/domain-types/form-result';
import { AdminResult } from '../common/domain-types/admin-result';
import { DAConfig } from '../common/domain-types/da-config';
import { AdminConfigType } from '../common/domain-types/admin-confit-type';
import { TreeControlComponent } from '../common/components/tree-control/tree-control.component';
import { AdminFormComponent } from '../admin-form/admin-form.component';
import { BehaviorSubject, forkJoin, Observable, Subscription } from 'rxjs';


@Component({
  selector: 'app-admin-tree',
  templateUrl: './admin-tree.component.html',
  styleUrls: ['./admin-tree.component.scss']
})
export class AdminTreeComponent implements OnInit {

  adminConfig: AdminConfig;
  treeSubject: BehaviorSubject<any>;
  tree: Observable<any>;
  treeDataSubject: BehaviorSubject<any[]>;
  treeData: Observable<any[]>;
  labelType = LabelType;
  daConfig: DAConfig;
  selectedNode: any;

  adminConfigType = AdminConfigType;

  @ViewChild(ConfirmDialogComponent)
  private confirmDialogComponent: ConfirmDialogComponent;

  @ViewChild(TreeControlComponent)
  private treeControl: TreeControlComponent;

  @ViewChild(AdminFormComponent, { static: false })
  private adminForm: AdminFormComponent;

  private formSubmitSubscription: Subscription;

  constructor(private _adminService: AdminService,
    private _alertService: AlertService,
    private _recordIdPipe: RecordIdPipe,
    @Inject(WINDOW) _window: any) {
    this.daConfig = _window.daConfig;
    this.treeSubject = new BehaviorSubject<any>({});
    this.treeDataSubject = new BehaviorSubject<any[]>([]);
    this.tree = this.treeSubject.asObservable();
    this.treeData = this.treeDataSubject.asObservable();
    this.treeSubject.subscribe(tree => {
      if (!this.adminConfig || !this.adminConfig.modelConfig || !this.adminConfig.modelConfig.treeConfig 
        || !this.adminConfig.modelConfig.treeConfig.childrenField
        || !this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase) return;
      this.treeDataSubject.next(tree[this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase])
    })
  }

  ngOnInit() {
    const adminConfig$ = this._adminService.getAdminConfig();
    const tree$ = this._adminService.getTree();
    forkJoin([adminConfig$, tree$]).subscribe(results => {
      this.adminConfig = results[0];
      this.onGetTree(results[1]);
    }, error => this._alertService.showMessage(AlertType.Error, 'Unable to get tree, please contact administrator'));

    if (this.formSubmitSubscription) {
      this.formSubmitSubscription.unsubscribe();
    }

    if (this.adminForm) {
      this.formSubmitSubscription = this.adminForm.submit$.subscribe(formResult => this.onFormSubmit(formResult));
    }
  }


  // getAdminConfig(): void {
  //   this._adminService.getAdminConfig()
  //     .subscribe(adminConfig => this.adminConfig = adminConfig, error => this.handleError(error));
  // }

  getTree(): void {
    this._adminService.getTree()
      .subscribe(tree => this.onGetTree(tree), error => this.handleError(error));
  }

  onChangePage(): void {
    this.getTree();
  }

  onGetTree(entityRecords: any): void {
    this.treeSubject.next(entityRecords);
  }

  onNodeDrop(node: any): void {
    // this.tree[this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase] = node as any[];
    let tree = this.treeSubject.getValue();
    const treeToUpdate = JSON.parse(JSON.stringify(tree));
    treeToUpdate[this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase] = node as any[];
    console.log(node);
    this._adminService.updateTree(treeToUpdate)
      .subscribe(response => this.onActionResult(response), error => this.handleError(error));
  }

  // onUpdateTree(adminResult: AdminResult) {
  //   if (adminResult.isSucceeded) {
  //     this.treeControl.rebuildTreeForData(adminResult.result[this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase]);
  //   } else {
  //     this.handleError('Error occured while updating the tree');
  //   }
  // }

  onNewItem(node: any): void {
    console.log(node);
  }

  onNodeDelete(node: any): void {
    // console.log(node);
    this.openDeleteConfirmationModal(node);
  }

  onNodeSelect(node: any): void {
    this.selectedNode = node;
    const nodeKey = node[this.adminConfig.modelConfig.keyField.fieldNameCamelCase];

    setTimeout(() => {

      if (this.formSubmitSubscription) {
        this.formSubmitSubscription.unsubscribe();
      }

      if (!nodeKey) {
        this.adminForm.initForm(nodeKey, this.selectedNode);
      } else {
        this.adminForm.initForm(nodeKey);
      }

      if (this.adminForm) {
        this.formSubmitSubscription = this.adminForm.submit$.subscribe(formResult => this.onFormSubmit(formResult));
      }
    });
  }

  openDeleteConfirmationModal(item: any) {
    this.confirmDialogComponent.openModal(item);
  }

  onYesToDelete(item: any): void {
    console.log('confirm');
    const itemId = this._recordIdPipe.transform(item, this.adminConfig.modelConfig.keyField);
    this.selectedNode = null;
    if (!itemId) { return; }
    this._adminService.deleteRecord(itemId)
      .subscribe(response => this.onActionResult(response), error => this.handleError(error));
  }

  onNoToDelete(): void {
    console.log('declined');
    this.getTree();
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
      this.treeControl.rebuildTreeForData(adminResult.result[this.adminConfig.modelConfig.treeConfig.childrenField.fieldNameCamelCase]);
    } else {
      this._alertService.showMessage(AlertType.Error, adminResult.errorMessage);
      this.getTree();
    }
  }

  onFormSubmit(formResult: FormResult) {
    if (formResult.isSucceeded) {
      this.getTree();
    }
  }

  handleError(message: string) {
    this._alertService.showMessage(AlertType.Error, message);
  }

  getBadge(item: any, field: Field): string {
    if (!field.fieldOption.labelOption) {
      return '';
    }

    if (!field.fieldOption.labelOption.parameters || !field.fieldOption.labelOption.parameters.paramFieldNameCamelCase) {
      if (field.fieldType === FieldType.CheckBox) {
        return item[field.fieldNameCamelCase] ? 'badge-primary' : 'badge-secondary';
      } else {
        return 'badge-light';
      }
    }

    return item[field.fieldOption.labelOption.parameters.paramFieldNameCamelCase];
  }

}
