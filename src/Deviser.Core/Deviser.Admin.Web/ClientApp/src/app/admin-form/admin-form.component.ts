import { Component, OnInit, Inject, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location, DOCUMENT } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable, BehaviorSubject } from 'rxjs';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { FormControlService } from '../common/services/form-control.service';
import { FormMode } from '../common/domain-types/form-mode';
import { Field } from '../common/domain-types/field';
import { ChildConfig } from '../common/domain-types/child-config';
import { Alert, AlertType } from '../common/domain-types/alert';
import { FormResult } from '../common/domain-types/form-result';
import { FormContext } from '../common/domain-types/form-context';
import { FormType } from '../common/domain-types/form-type';
import { DAConfig } from '../common/domain-types/da-config';
import { WINDOW } from '../common/services/window.service';
import { AdminConfigType } from '../common/domain-types/admin-confit-type';
import { FormBehaviour } from '../common/domain-types/form-behaviour';
import { OpenUrlAction } from '../common/domain-types/open-url-action';
import { AlertService } from '../common/services/alert.service';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {
  adminConfig: AdminConfig;
  alerts: Alert[];
  record: any;
  adminForm: FormGroup;
  formMode: FormMode;
  FormMode = FormMode;
  selectedChildConfig: ChildConfig;
  formTabs: FormContext[];
  selectedFormTab: FormContext;
  // childFormContexts: { [key: string]: FormContext }

  formType = FormType;
  submitSubject: BehaviorSubject<FormResult> = new BehaviorSubject<any>({});
  daConfig: DAConfig;
  adminConfigType = AdminConfigType;
  activeChildConfigs: ChildConfig[];

  constructor(private route: ActivatedRoute,
    private adminService: AdminService,
    private alertService: AlertService,
    @Inject(DOCUMENT) private document: Document,
    private formControlService: FormControlService,
    private fb: FormBuilder,
    private location: Location,
    @Inject(WINDOW) private window: any) {
    this.alerts = [];
    this.daConfig = window.daConfig;
    // this.childFormContexts;
    this.alertService.alerts.subscribe(alert => {
      if(alert){
        this.alerts.push(alert)
      }
    });
  }

  ngOnInit() {
    // this.getAdminConfig();
    // this.getRecord();
    const id = this.route.snapshot.paramMap.get('id');
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
      const adminConfig$ = this.adminService.getAdminConfig();
      const record$ = this.adminService.getRecord(itemId);
      forkJoin([adminConfig$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetAdminConfig(results[0]);
      }, error => {
        const alert: Alert = {
          alertType: AlertType.Error,
          message: 'Unable to get this item, please contact administrator',
          timeout: 5000
        }
        this.alertService.addAlert(alert);
      });
    } else if (this.formMode === FormMode.Create) {
      this.record = record;
      this.adminService.getAdminConfig()
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


      this.adminForm = this.formControlService.toFormGroup(adminConfig, this.formMode, this.record);
      this.formTabs.push({
        formGroup: this.adminForm,
        formConfig: this.adminConfig.modelConfig.formConfig,
        formName: this.adminConfig.modelType,
        formTitle: this.adminConfig.modelType,
        formType: this.formType.MainForm,
        keyField: this.adminConfig.modelConfig.keyField,
        formMode: this.formMode,
        lookUps: this.adminConfig.lookUps
      });
      this.selectedFormTab = this.formTabs[0];
      if (this.formMode == FormMode.Update && this.adminConfig.modelConfig.customForms) {
        for (let customFormName in this.adminConfig.modelConfig.customForms) {
          let customForm = this.adminConfig.modelConfig.customForms[customFormName];
          let customFormGroup = this.formControlService.toFormGroupWithFormConfig(customForm.formConfig, this.formMode, customForm.keyField, this.record);
          this.formTabs.push({
            formGroup: customFormGroup,
            formConfig: customForm.formConfig,
            formName: customFormName,
            formTitle: customForm.formConfig.formOption.formTitle,
            formType: this.formType.CustomForm,
            keyField: customForm.keyField,
            formMode: this.formMode,
            lookUps: this.adminConfig.lookUps
          });
        }
      }

      setTimeout(() => {
        if (this.record) {
          this.patchFormValue(this.record);
        }
      });
    }
  }

  onSubmit(): void {
    // TODO: Use EventEmitter with form value
    console.warn(this.adminForm.value);
    if (this.formMode === FormMode.Create) {
      this.adminService.createRecord(this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    } else if (this.formMode === FormMode.Update) {
      this.adminService.updateRecord(this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onCustomFormSubmit(formTab: any) {
    this.adminService.customFormSubmit(formTab.formName, formTab.formGroup.value)
      .subscribe(formValue => this.onActionResult(formValue));
  }

  onAction(formName: string, actionName: string): void {
    if (formName) {
      this.adminService.executeCustomFormAction(formName, actionName, this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    } else {
      this.adminService.executeMainFormAction(actionName, this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onActionResult(formValue: FormResult): void {
    this.submitSubject.next(formValue);
    if (formValue && formValue.isSucceeded) {
      let alert: Alert = {
        alertType: AlertType.Success,
        message: formValue.successMessage,
        timeout: 5000
      }
      this.alertService.addAlert(alert);
      if (formValue.formBehaviour === FormBehaviour.RedirectToGrid && this.adminConfig.adminConfigType === AdminConfigType.GridAndForm) {
        const openUrlAction = formValue.successAction as OpenUrlAction;
        if (openUrlAction) {
          console.log(openUrlAction);
          setTimeout(() => {
            this.document.location.href = `${this.daConfig.basePath}${openUrlAction.url}`;
          }, openUrlAction.openAfterSec * 1000);
        }
        this.goBack();
      }
    }
    else {
      let alert: Alert = {
        alertType: AlertType.Error,
        message: formValue.successMessage,
        timeout: 5000
      }
      this.alertService.addAlert(alert);
    }
  }

  patchFormValue(formValue: any): void {
    if (formValue) {
      this.adminForm.patchValue(formValue);
    }
    else {
      let alert: Alert = {
        alertType: AlertType.Error,
        message: "Unable to update/save this item, please contact administrator",
        timeout: 5000
      }
      this.alertService.addAlert(alert);
    }

  }

  getChildForm(childFieldName: string): FormGroup {
    let childFormObj = this.adminForm.controls[childFieldName].value;
    let childForm: FormGroup = this.fb.group(childFormObj);
    return childForm;
  }

  selectFormTab(formTab) {
    this.selectedFormTab = formTab;
  }

  selectChildFormTab(config) {
    this.selectedChildConfig = config;
  }

  goBack(): void {
    this.location.back();
  }

}
