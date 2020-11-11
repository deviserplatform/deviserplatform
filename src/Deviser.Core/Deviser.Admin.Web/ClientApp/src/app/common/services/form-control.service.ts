import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminConfig } from '../domain-types/admin-config';
import { Field } from '../domain-types/field';
import { FieldType } from '../domain-types/field-type';
import { KeyField } from '../domain-types/key-field';
import { KeyFieldType } from '../domain-types/key-field-type';
import { ModelConfig } from '../domain-types/model-config';
import { FormConfig } from '../domain-types/form-config';
import { FormMode } from '../domain-types/form-mode';
import { AdminConfigType } from '../domain-types/admin-confit-type';


@Injectable({
  providedIn: 'root'
})
export class FormControlService {

  constructor(private fb: FormBuilder) { }

  getLookUpKeys(lookUpGeneric: any) {
    return Object.keys(lookUpGeneric[0].key);
  }

  // getLookUp(lookUpGeneric: any): any[] {
  //   if (lookUpGeneric) {
  //     const keyNames = this.getLookUpKeys(lookUpGeneric);
  //     const lookUp = [];

  //     lookUpGeneric.forEach(item => {
  //       const propValue: any = {};
  //       //copy display name from generic lookup  
  //       propValue.displayName = item.displayName;

  //       keyNames.forEach(keyName => {
  //         propValue[keyName] = item.key[keyName]
  //       });

  //       lookUp.push(propValue);
  //     });
  //     return lookUp;
  //   }
  // }

  getSelectedItemFor(lookUp: any[], lookUpKeys: string[], controlVal: any): any {
    if (!lookUp || !lookUpKeys || !controlVal) return;
    let selectedItem = lookUp.find(lookUp => {
      let isMatch = false;
      for (let i = 0; i < lookUpKeys.length; i++) {
        let prop = lookUpKeys[i];
        isMatch = lookUp[prop] === controlVal[prop];
        if (isMatch)
          return isMatch;
      }
      return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
    });
    return selectedItem;
  }


  getSelectedItemsFor(lookUp: any[], lookUpKeys: string[], controlVal: any) {
    if (!lookUp || !lookUpKeys || !controlVal || controlVal.length <= 0) return;

    let selectedItems: any[] = [];

    //Parse control value and set displayName
    controlVal.forEach(item => {
      let masterItem = lookUp.find(lookUp => {
        let isMatch = false;
        for (let i = 0; i < lookUpKeys.length; i++) {
          let prop = lookUpKeys[i];
          isMatch = lookUp[prop] === item[prop];
          if (isMatch)
            return isMatch;
        }
        return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
      });
      selectedItems.push(masterItem);
      // item.displayName = masterItem.displayName; //Not required, since selected items are patched directly
    });
    return selectedItems;
  }

  toFormGroup(adminConfig: AdminConfig, formMode: FormMode, record: any = null): FormGroup {
    let formObj: any = {};

    formObj = this.getFormGroup(adminConfig.modelConfig, formMode, formObj, record);

    if (adminConfig.childConfigs && adminConfig.childConfigs.length > 0) {

      adminConfig.childConfigs.forEach(childConfig => {
        // let childForm: any[] = [];
        // let childRecords = record[childConfig.field.fieldNameCamelCase] ? record[childConfig.field.fieldNameCamelCase] : [];
        formObj[childConfig.field.fieldNameCamelCase] = this.getFormControl(childConfig.field, record, true);
      });
    }

    if (adminConfig.adminConfigType === AdminConfigType.TreeAndForm) {
      const recordKeys = Object.keys(record);
      const formObjKeys = Object.keys(formObj);
      if (recordKeys.length > 0) {
        const keysToBeAdded = recordKeys.filter(rk => formObjKeys.indexOf(rk) < 0);
        if (keysToBeAdded && keysToBeAdded.length > 0) {
          keysToBeAdded.forEach(k => {
            formObj[k] = new FormControl(record[k]);
          });
        }
      }
    }

    return this.fb.group(formObj);
  }

  toFormGroupWithModelConfig(modelConfig: ModelConfig, formMode: FormMode, record: any = null): FormGroup {
    let formObj: any = {};
    formObj = this.getFormGroup(modelConfig, formMode, formObj, record);
    return this.fb.group(formObj);
  }

  toFormGroupWithFormConfig(formConfig: FormConfig, formMode: FormMode, keyField: KeyField, record: any = null) {
    let formObj: any = {};
    formObj = this.buildAndGetFormGroup(formConfig, formMode, keyField, formObj, record);
    return this.fb.group(formObj);
  }

  private getFormGroup(modelConfig: ModelConfig, formMode: FormMode, formObj: any, record: any, forChildRecords: boolean = false) {
    return this.buildAndGetFormGroup(modelConfig.formConfig, formMode, modelConfig.keyField, formObj, record, forChildRecords);
  }

  private buildAndGetFormGroup(formConfig: FormConfig, formMode: FormMode, keyField: KeyField, formObj: any, record: any, forChildRecords: boolean = false) {
    if (keyField) {
      formObj[keyField.fieldNameCamelCase] = this.getKeyControl(keyField, record);
    }
    formObj = this.getFormModel(formConfig, formMode, formObj, record, forChildRecords);
    return formObj;
  }

  private getFormModel(formConfig: FormConfig, formMode: FormMode, adminForm: any, record: any, forChildRecords: boolean): any {
    if (formConfig &&
      formConfig.fieldConfig &&
      formConfig.fieldConfig.fields &&
      formConfig.fieldConfig.fields.length > 0) {
      const fields = formConfig.fieldConfig.fields;

      fields.forEach(fieldRow => {
        if (fieldRow && fieldRow.length > 0) {
          fieldRow.forEach(field => {
            if (field.fieldOption.showIn === FormMode.Both || field.fieldOption.showIn === formMode) {
              adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record, forChildRecords);
            }
          });
        }
      });

      // adminForm['fields'] = this.fb.group(fieldsGroup); 
      // adminForm =  this.fb.group(fieldsGroup);

    } else if (formConfig &&
      formConfig.fieldSetConfig &&
      formConfig.fieldSetConfig.fieldSets &&
      formConfig.fieldSetConfig.fieldSets.length > 0) {
      const fieldSets = formConfig.fieldSetConfig.fieldSets;
      fieldSets.forEach(fieldSet => {
        if (fieldSet && fieldSet.fields && fieldSet.fields.length > 0) {
          fieldSet.fields.forEach(fieldRow => {
            if (fieldRow && fieldRow.length > 0) {
              fieldRow.forEach(field => {
                if (field.fieldOption.showIn == FormMode.Both || field.fieldOption.showIn == formMode) {
                  adminForm[field.fieldNameCamelCase] = this.getFormControl(field, record, forChildRecords);
                }
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
    } else if (field.fieldType === FieldType.CheckBoxMatrix) {
      controlValue = controlValue && Array.isArray(controlValue) ? controlValue : [];
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
