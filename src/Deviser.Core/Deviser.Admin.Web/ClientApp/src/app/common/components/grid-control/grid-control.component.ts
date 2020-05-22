import { Component, OnInit, Input } from '@angular/core';
import { Field } from '../../domain-types/field';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';
import { FieldType } from '../../domain-types/field-type';
import { DatePipe } from '@angular/common';
import { LabelType } from '../../domain-types/label-type';

@Component({
  selector: 'app-grid-control',
  templateUrl: './grid-control.component.html',
  styleUrls: ['./grid-control.component.scss']
})
export class GridControlComponent implements OnInit {

  @Input() field: Field;
  @Input() lookUps: LookUpDictionary;
  @Input() item: any;

  labelType = LabelType;

  constructor(private datePipe: DatePipe) { }

  get itemText(): string {

    if (!this.field || !this.item) {
      return '';
    }

    const value = this.item[this.field.fieldNameCamelCase];
    const fieldOption = this.field.fieldOption;
    switch (this.field.fieldType) {
      case FieldType.Currency:
      case FieldType.Number:
      case FieldType.EmailAddress:
      case FieldType.Phone:
      case FieldType.RichText:
      case FieldType.Static:
      case FieldType.TextArea:
      case FieldType.TextBox:
      case FieldType.Url:
        return value;
      case FieldType.CheckBox:
        const trueText = fieldOption.isTrue ? fieldOption.isTrue : 'True';
        const falseText = fieldOption.isFalse ? fieldOption.isFalse : 'False';
        return (typeof value === 'boolean') && value ? trueText : falseText;
      case FieldType.Date:
      case FieldType.DateTime:
      case FieldType.Time:
        return fieldOption.format ? this.datePipe.transform(value, fieldOption.format) : value;
      case FieldType.MultiSelect:
      case FieldType.MultiSelectCheckBox:
        const displayItems = this.getM2mControlValue(value);
        return displayItems && displayItems.length > 0 ? displayItems.join(',') : '';
      case FieldType.Select:
        const displayName = this.getM2oControlValue(value);
        return displayName ? displayName : '';
    }
  }

  ngOnInit(): void {
  }


  getBadge(item: any, field: Field): string {
    if (!field.fieldOption.labelOption) {
      return '';
    }

    if (!field.fieldOption.labelOption.parameters || !field.fieldOption.labelOption.parameters.paramFieldNameCamelCase) {
      if (field.fieldType === FieldType.CheckBox) {
        return item[field.fieldNameCamelCase] ? 'badge-primary' : 'badge-secondary';
      }
      else {
        return 'badge-light';
      }
    }

    return item[field.fieldOption.labelOption.parameters.paramFieldNameCamelCase];
  }

  private getM2mControlValue(controlVal: any): string[] {
    if (!controlVal || controlVal.length <= 0) {
      return null;
    }

    const lookUpGeneric = this.lookUps.lookUpData[this.field.fieldNameCamelCase]
    const lookUp = this.getLookUp(lookUpGeneric);
    const keyNames = Object.keys(lookUpGeneric[0].key);
    const selectedItems: string[] = [];

    controlVal.forEach(item => {
      const masterItem = lookUp.find(lookUpItem => {
        let isMatch = false;
        for (let i = 0; i < keyNames.length; i++) {
          const prop = keyNames[i];
          isMatch = lookUpItem[prop] === item[prop];
          if (isMatch) {
            return isMatch;
          }
        }
        return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
      });
      selectedItems.push(masterItem.displayName);
      // item.displayName = masterItem.displayName; //Not required, since selected items are patched directly
    });
    return selectedItems;

  }

  private getM2oControlValue(controlVal: any): string {
    if (!controlVal) {
      return null;
    }

    const lookUpGeneric = this.lookUps.lookUpData[this.field.fieldNameCamelCase]
    const lookUp = this.getLookUp(lookUpGeneric);
    const keyNames = Object.keys(lookUpGeneric[0].key);


    const masterItem = lookUp.find(lookUpItem => {
      let isMatch = false;
      for (let i = 0; i < keyNames.length; i++) {
        const prop = keyNames[i];
        isMatch = lookUpItem[prop] === controlVal[prop];
        if (isMatch) {
          return isMatch;
        }
      }
      return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
    });
    return masterItem ? masterItem.displayName : null;
  }

  private getLookUp(lookUpGeneric: any): any[] {
    if (lookUpGeneric) {
      const keyNames = Object.keys(lookUpGeneric[0].key);
      const lookUp = [];

      lookUpGeneric.forEach(item => {
        const propValue: any = {};
        //copy display name from generic lookup  
        propValue.displayName = item.displayName;

        keyNames.forEach(keyName => {
          propValue[keyName] = item.key[keyName]
        });

        lookUp.push(propValue);
      });
      return lookUp;
    }
  }

}
