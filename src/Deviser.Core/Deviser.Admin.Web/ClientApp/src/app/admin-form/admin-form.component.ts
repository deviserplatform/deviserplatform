import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location, DOCUMENT } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable, BehaviorSubject, Subject } from 'rxjs';
import { AlertType } from 'deviser-shared';
import { AlertService } from 'deviser-shared';


import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { FormControlService } from '../common/services/form-control.service';
import { FormMode } from '../common/domain-types/form-mode';
import { Field } from '../common/domain-types/field';
import { ChildConfig } from '../common/domain-types/child-config';

import { FormResult } from '../common/domain-types/form-result';
import { FormContext } from '../common/domain-types/form-context';
import { FormType } from '../common/domain-types/form-type';
import { DAConfig } from '../common/domain-types/da-config';
import { WINDOW } from '../common/services/window.service';
import { AdminConfigType } from '../common/domain-types/admin-confit-type';
import { FormBehaviour } from '../common/domain-types/form-behaviour';
import { OpenUrlAction } from '../common/domain-types/open-url-action';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {
  adminConfig: AdminConfig;
  record: any;
  adminForm: FormGroup;
  formMode: FormMode;
  FormMode = FormMode;
  selectedChildConfig: ChildConfig;
  formTabs: FormContext[];
  selectedFormTab: FormContext;
  // childFormContexts: { [key: string]: FormContext }

  formType = FormType;
  submitSubject: Subject<FormResult> = new Subject<any>();
  submit$ = this.submitSubject.asObservable();
  daConfig: DAConfig;
  adminConfigType = AdminConfigType;
  activeChildConfigs: ChildConfig[];

  constructor(private _route: ActivatedRoute,
    private _adminService: AdminService,
    private _alertService: AlertService,
    @Inject(DOCUMENT) private _document: Document,
    private _formControlService: FormControlService,
    private _fb: FormBuilder,
    private _location: Location,
    @Inject(WINDOW) private _window: any) {
    this.daConfig = _window.daConfig;
    // this.childFormContexts;
  }

  ngOnInit() {
    // this.getAdminConfig();
    // this.getRecord();
    const id = this._route.snapshot.paramMap.get('id');
    this.initForm(id);
  }

  initForm(itemId: string, record: any = {}): void {
    if (this.daConfig.adminConfigType === AdminConfigType.GridAndForm ||
      this.daConfig.adminConfigType === AdminConfigType.TreeAndForm) {
      this.formMode = itemId ? FormMode.Update : FormMode.Create;
    } else {
      itemId = 'nothing';
      this.formMode = FormMode.Update;
    }

    if (this.formMode === FormMode.Update) {
      const adminConfig$ = this._adminService.getAdminConfig();
      const record$ = this._adminService.getRecord(itemId);
      forkJoin([adminConfig$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetAdminConfig(results[0]);
      }, error => this._alertService.showMessage(AlertType.Error, 'Unable to get this item, please contact administrator'));
    } else if (this.formMode === FormMode.Create) {
      this.record = record;
      this._adminService.getAdminConfig()
        .subscribe(adminConfig => this.onGetAdminConfig(adminConfig));
    }

    // this.adminService.getAdminConfig()
    //   .subscribe(adminConfig => this.onGetAdminConfig(adminConfig, itemId, record));

  }

  onGetAdminConfig(adminConfig: AdminConfig): void {
    this.formTabs = [];
    if (adminConfig) {
      this.adminConfig = adminConfig;

      if (this.adminConfig.childConfigs.length > 0) {
        this.activeChildConfigs = this.adminConfig.childConfigs.filter(c => c.isShown);
      }

      if (this.activeChildConfigs && this.activeChildConfigs.length > 0) {
        this.selectedChildConfig = this.activeChildConfigs[0];
        // this.childFormContexts = {};
        // for (let childConfig of this.adminConfig.childConfigs) {

        //   let childForm = this.formControlService.toFormGroupWithModelConfig(childConfig.modelConfig, this.formMode, {});
        //   let childFormName = childConfig.field.fieldNameCamelCase;
        //   this.childFormContexts[childFormName] = {
        //     formGroup: childForm,
        //     formConfig: childConfig.modelConfig.formConfig,
        //     formName: childFormName,
        //     formTitle: childFormName,
        //     formType: this.formType.ChildForm,
        //     keyField: childConfig.modelConfig.keyField,
        //     formMode: this.formMode,
        //     lookUps: this.adminConfig.lookUps
        //   };
        // }
      }


      this.adminForm = this._formControlService.toFormGroup(adminConfig, this.formMode, this.record);
      this.formTabs.push({
        formGroup: this.adminForm,
        formConfig: this.adminConfig.modelConfig.formConfig,
        formName: this.adminConfig.modelType,
        formTitle: this.adminConfig.modelConfig.formConfig.title ? this.adminConfig.modelConfig.formConfig.title : this.adminConfig.modelType,
        formType: this.formType.MainForm,
        keyField: this.adminConfig.modelConfig.keyField,
        formMode: this.formMode,
        lookUps: this.adminConfig.lookUps
      });
      this.selectedFormTab = this.formTabs[0];
      if (this.formMode == FormMode.Update && this.adminConfig.modelConfig.customForms) {
        for (let customFormName in this.adminConfig.modelConfig.customForms) {
          let customForm = this.adminConfig.modelConfig.customForms[customFormName];
          let customFormGroup = this._formControlService.toFormGroupWithFormConfig(customForm.formConfig, this.formMode, customForm.keyField, this.record);
          this.formTabs.push({
            formGroup: customFormGroup,
            formConfig: customForm.formConfig,
            formName: customFormName,
            formTitle: customForm.formConfig.title ? customForm.formConfig.title : customForm.formName,
            formType: this.formType.CustomForm,
            keyField: customForm.keyField,
            formMode: this.formMode,
            lookUps: this.adminConfig.lookUps
          });
        }
      }

      // setTimeout(() => {
      //   if (this.record) {
      //     this.patchFormValue(this.record);
      //   }
      // });
    }
  }

  onSubmit(): void {
    // TODO: Use EventEmitter with form value
    console.warn(this.adminForm.value);
    if (this.formMode === FormMode.Create) {
      this._adminService.createRecord(this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    } else if (this.formMode === FormMode.Update) {
      this._adminService.updateRecord(this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onCustomFormSubmit(formTab: any) {
    this._adminService.customFormSubmit(formTab.formName, formTab.formGroup.value)
      .subscribe(formValue => this.onActionResult(formValue));
  }

  onAction(formTab: any, actionName: string): void {
    if (formTab.formType == this.formType.CustomForm) {
      this._adminService.executeCustomFormAction(formTab.formName, actionName, this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    } else {
      this._adminService.executeMainFormAction(actionName, this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onActionResult(formValue: FormResult): void {
    this.submitSubject.next(formValue);
    if (formValue && formValue.isSucceeded) {
      this._alertService.showMessage(AlertType.Success, formValue.successMessage);
      if (formValue.formBehaviour === FormBehaviour.RedirectToGrid && this.adminConfig.adminConfigType === AdminConfigType.GridAndForm) {
        const openUrlAction = formValue.successAction as OpenUrlAction;
        if (openUrlAction) {
          console.log(openUrlAction);
          setTimeout(() => {
            this._document.location.href = `${this.daConfig.basePath}${openUrlAction.url}`;
          }, openUrlAction.openAfterSec * 1000);
        }
        this.goBack();
      }
    }
    else {
      this._alertService.showMessage(AlertType.Error, formValue.errorMessage);
    }
  }

  patchFormValue(formValue: any): void {
    if (formValue) {
      this.adminForm.patchValue(formValue);
    }
    else {
      this._alertService.showMessage(AlertType.Error, 'Unable to update/save this item, please contact administrator');
    }

  }

  getChildForm(childFieldName: string): FormGroup {
    let childFormObj = this.adminForm.controls[childFieldName].value;
    let childForm: FormGroup = this._fb.group(childFormObj);
    return childForm;
  }

  selectFormTab(formTab) {
    this.selectedFormTab = formTab;
  }

  selectChildFormTab(config) {
    this.selectedChildConfig = config;
  }

  goBack(): void {
    this._location.back();
  }

}
