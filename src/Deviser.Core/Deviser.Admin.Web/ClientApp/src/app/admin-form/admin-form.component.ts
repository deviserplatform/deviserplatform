import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';

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

  daConfig: DAConfig;
  adminConfigType = AdminConfigType

  constructor(private route: ActivatedRoute,
    private adminService: AdminService,
    private formControlService: FormControlService,
    private fb: FormBuilder,
    private location: Location,
    @Inject(WINDOW) private window: any) {
    this.alerts = [];
    this.formTabs = [];
    this.daConfig = window.daConfig;
    // this.childFormContexts;
  }

  ngOnInit() {
    // this.getAdminConfig();
    // this.getRecord();
    this.getData();
  }

  getData(): void {
    let id = this.route.snapshot.paramMap.get('id');

    if (this.daConfig.adminConfigType == AdminConfigType.GridAndForm) {
      this.formMode = id ? FormMode.Update : FormMode.Create;
    }
    else {
      id = 'nothing';
      this.formMode = FormMode.Update;
    }



    if (this.formMode === FormMode.Update) {
      const adminConfig$ = this.adminService.getAdminConfig();
      const record$ = this.adminService.getRecord(id);
      forkJoin([adminConfig$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetAdminConfig(results[0]);
      }, error => {
        let alert: Alert = {
          alterType: AlertType.Error,
          message: "Unable to get this item, please contact administrator",
          timeout: 5000
        }
        this.alerts.push(alert);
      });
    } else if (this.formMode === FormMode.Create) {
      this.record = {};
      this.adminService.getAdminConfig()
        .subscribe(adminConfig => this.onGetAdminConfig(adminConfig));
    }

  }

  onGetAdminConfig(adminConfig: AdminConfig): void {
    if (adminConfig) {
      this.adminConfig = adminConfig;


      if (this.adminConfig.childConfigs.length > 0) {
        this.selectedChildConfig = this.adminConfig.childConfigs[0];
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
    }
    else {
      this.adminService.executeMainFormAction(actionName, this.adminForm.value)
        .subscribe(formValue => this.onActionResult(formValue));
    }
  }

  onActionResult(formValue: FormResult): void {
    if (formValue && formValue.isSucceeded) {
      if (formValue.formBehaviour == FormBehaviour.RedirectToGrid) {
        this.goBack();
      }
      else {
        let alert: Alert = {
          alterType: AlertType.Success,
          message: formValue.successMessage,
          timeout: 5000
        }
        this.alerts.push(alert);
      }
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

  patchFormValue(formValue: any): void {
    if (formValue) {
      this.adminForm.patchValue(formValue);
    }
    else {
      let alert: Alert = {
        alterType: AlertType.Error,
        message: "Unable to update/save this item, please contact administrator",
        timeout: 5000
      }
      this.alerts.push(alert);
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
