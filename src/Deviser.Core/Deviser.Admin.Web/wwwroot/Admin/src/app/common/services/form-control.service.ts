import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminConfig } from '../domain-types/admin-config';
import { Field } from '../domain-types/field';
import { FieldType } from '../domain-types/field-type';
import { KeyField } from '../domain-types/key-field';
import { KeyFieldType } from '../domain-types/key-field-type';
import { FormConfig } from '../domain-types/form-config';


@Injectable({
  providedIn: 'root'
})
export class FormControlService {

  constructor(private fb: FormBuilder) { }

  toFormGroup(adminConfig: AdminConfig, record: any = null): FormGroup {
    let adminForm: any = {};

    adminForm = this.getFormGroup(adminConfig.formConfig, adminForm, record);

    if (adminConfig.childConfigs && adminConfig.childConfigs.length > 0) {

      adminConfig.childConfigs.forEach(childConfig => {
        // let childForm: any[] = [];
        // let childRecords = record[childConfig.field.fieldNameCamelCase] ? record[childConfig.field.fieldNameCamelCase] : [];
        adminForm[childConfig.field.fieldNameCamelCase] = this.getFormControl(childConfig.field, record, true);
      });
    }

    return this.fb.group(adminForm);
  }

  toChildFormGroup(formConfig: FormConfig, record: any = null): FormGroup {
    let adminForm: any = {};
    adminForm = this.getFormGroup(formConfig, adminForm, record);
    return this.fb.group(adminForm);
  }

  private getFormGroup(formConfig: FormConfig, adminForm: any, record: any, forChildRecords: boolean = false) {
    if (formConfig.keyFields) {
      let pkFields = formConfig.keyFields.filter(kf => kf.keyFieldType == KeyFieldType.PrimaryKey);
      pkFields.forEach(field => {
        adminForm[field.fieldNameCamelCase] = this.getKeyControl(field, record);
      });
    }
    adminForm = this.getFormEnity(formConfig, adminForm, record, forChildRecords);
    return adminForm;
  }

  private getFormEnity(formConfig: FormConfig, adminForm: any, record: any, forChildRecords: boolean): any {
    if (formConfig && formConfig.fieldConfig &&
      formConfig.fieldConfig.fields && formConfig.fieldConfig.fields.length > 0) {
      const fields = formConfig.fieldConfig.fields;

      fields.forEach(fieldRow => {
        if (fieldRow && fieldRow.length > 0) {
          fieldRow.forEach(field => {
            adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record, forChildRecords);
          });
        }
      });

      // adminForm['fields'] = this.fb.group(fieldsGroup); 
      // adminForm =  this.fb.group(fieldsGroup);

    } else if (formConfig.fieldSetConfig && formConfig.fieldSetConfig.fieldSets &&
      formConfig.fieldSetConfig.fieldSets.length > 0) {
      const fieldSets = formConfig.fieldSetConfig.fieldSets;
      fieldSets.forEach(fieldSet => {
        if (fieldSet && fieldSet.fields && fieldSet.fields.length > 0) {
          fieldSet.fields.forEach(fieldRow => {
            if (fieldRow && fieldRow.length > 0) {

              fieldRow.forEach(field => {
                adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record, forChildRecords);
              });
            }

          });
        }
      });
    }

    return adminForm;
  }

  private getFormControl(field: Field, record: any, forChildRecords: boolean): FormControl {
    let formControl: FormControl;

    let controlValue = null;
    let defaultValue: any;
    defaultValue = forChildRecords ? [] : '';
    controlValue = record && record[field.fieldNameCamelCase] ? record[field.fieldNameCamelCase] : defaultValue;

    if (field.fieldType === FieldType.DateTime) {
      controlValue = controlValue ? new Date(controlValue) : new Date();
    }

    formControl = field.fieldOption && field.fieldOption.isRequired ? new FormControl(controlValue, Validators.required) : new FormControl(controlValue);

    return formControl;
  }

  private getKeyControl(keyField: KeyField, record: any) {
    let formControl: FormControl;
    let hasValue = record && record[keyField.fieldNameCamelCase];
    let controlValue = hasValue ? record[keyField.fieldNameCamelCase] : '';
    if (hasValue) {
      formControl = new FormControl(controlValue, Validators.required);
    }
    else {
      formControl = new FormControl(controlValue);
    }

    return formControl;
  }

}
