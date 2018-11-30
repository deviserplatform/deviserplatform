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

  toFormGroup(adminConfig: AdminConfig): FormGroup {
    let adminForm: any = {};

    if (adminConfig && adminConfig.fieldConfig &&
      adminConfig.fieldConfig.fields && adminConfig.fieldConfig.fields.length > 0) {
      let fields = adminConfig.fieldConfig.fields;
      adminForm['fields'] = this.fb.group({});

      fields.forEach(fieldRow => {
        if (fieldRow && fieldRow.length > 0) {
          fieldRow.forEach(field => {
            adminForm['fields'][field.fieldNameCamelCase] = this.getFormControl(field);
          });
        }
      });

    }
    else if (adminConfig && adminConfig.fieldSetConfig &&
      adminConfig.fieldSetConfig.fieldSets && adminConfig.fieldSetConfig.fieldSets.length > 0) {
      let fieldSets = adminConfig.fieldSetConfig.fieldSets;
      fieldSets.forEach(fieldSet => {
        if (fieldSet && fieldSet.fields && fieldSet.fields.length > 0) {
          adminForm['fieldSet'] = this.fb.group({});
          adminForm['fieldSet'][fieldSet.groupName] = this.fb.group({});

          fieldSet.fields.forEach(fieldRow => {
            if (fieldRow && fieldRow.length > 0) {

              fieldRow.forEach(field => {
                adminForm['fieldSet'][fieldSet.groupName][field.fieldNameCamelCase] = this.getFormControl(field);
              });
            }

          });
        }        
      });
    }

    return new FormGroup(adminForm);
  }

  private getFormControl(field: Field): FormControl {
    let formControl: FormControl;

    formControl = field.fieldOption.isReadOnly ? new FormControl('', Validators.required)
      : new FormControl('');

    return formControl;
  }

}
