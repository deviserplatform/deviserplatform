import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminConfig } from '../domain-types/admin-config';
import { Field } from '../domain-types/field';
import { FieldType } from '../domain-types/field-type';


@Injectable({
  providedIn: 'root'
})
export class FormControlService {

  constructor(private fb: FormBuilder) { }

  toFormGroup(adminConfig: AdminConfig, record: any = null): FormGroup {
    const adminForm: any = {};


    if (adminConfig && adminConfig.keyFields) {
      adminConfig.keyFields.forEach(field => {
        adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record);
      });
    }

    if (adminConfig && adminConfig.fieldConfig &&
      adminConfig.fieldConfig.fields && adminConfig.fieldConfig.fields.length > 0) {
      const fields = adminConfig.fieldConfig.fields;

      fields.forEach(fieldRow => {
        if (fieldRow && fieldRow.length > 0) {
          fieldRow.forEach(field => {
            adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record);
          });
        }
      });

      // adminForm['fields'] = this.fb.group(fieldsGroup);
      // adminForm =  this.fb.group(fieldsGroup);

    } else if (adminConfig && adminConfig.fieldSetConfig &&
      adminConfig.fieldSetConfig.fieldSets && adminConfig.fieldSetConfig.fieldSets.length > 0) {
      const fieldSets = adminConfig.fieldSetConfig.fieldSets;
      fieldSets.forEach(fieldSet => {
        if (fieldSet && fieldSet.fields && fieldSet.fields.length > 0) {
          fieldSet.fields.forEach(fieldRow => {
            if (fieldRow && fieldRow.length > 0) {

              fieldRow.forEach(field => {
                adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record);
              });
            }

          });
        }
      });
    }

    return this.fb.group(adminForm);
  }

  private getFormControl(field: Field, record: any): FormControl {
    let formControl: FormControl;

    let controlValue = record && record[field.fieldNameCamelCase] ? record[field.fieldNameCamelCase] : '';

    if (field.fieldType === FieldType.DateTime) {
      controlValue = controlValue ? new Date(controlValue) : new Date();
    }

    formControl = field.fieldOption && field.fieldOption.isReadOnly ? new FormControl(controlValue, Validators.required)
      : new FormControl(controlValue);

    return formControl;
  }

}
