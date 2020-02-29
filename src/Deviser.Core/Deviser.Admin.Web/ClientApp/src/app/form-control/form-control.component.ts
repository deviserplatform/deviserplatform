import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { repeat } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ValidationType } from '../common/domain-types/validation-type';
import { EmailExistValidator } from '../common/validators/async-email-exist.validator';
import { PasswordValidator } from '../common/validators/async-password.validator';
import { UserExistValidator } from '../common/validators/async-user-exist.validator';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

import { RelatedField } from '../common/domain-types/related-field';
import { RelationType } from '../common/domain-types/relation-type';
import { CustomValidator } from '../common/validators/async-custom.validator';
import { FormType } from '../common/domain-types/form-type';

@Component({
  selector: 'app-form-control',
  templateUrl: './form-control.component.html',
  styleUrls: ['./form-control.component.scss']
})
export class FormControlComponent implements OnInit {

  // @Input() entityType: string;
  @Input() field: Field;
  @Input() form: FormGroup;
  @Input() isShown: boolean;
  @Input() isDisabled: boolean;
  @Input() isValidate: boolean;
  // @Input() keyFields: Field[];
  @Input() lookUps: LookUpDictionary;
  @Input() formType: FormType;
  @Input() formName: string;

  //To access FieldType enum
  fieldType = FieldType;
  _lookUpData: any[];
  Editor = ClassicEditor;

  constructor(private emailExistValidator: EmailExistValidator,
    private passwordValidator: PasswordValidator,
    private userExistValidator: UserExistValidator,
    private customValidator: CustomValidator) {
    this._lookUpData = [];


  }

  ngOnInit() {
    this.parseControlValue();
  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes.isValidate && changes.isValidate.currentValue != null) {
      //Dynamic validation can be changed only when this field is not required by backend
      if (!this.field.fieldOption.isRequired || this.field.fieldOption.validationType) {
        this.onIsValidateChange(changes.isValidate.currentValue);
      }
    }
  }

  parseControlValue() {
    if (this.field && this.field.fieldOption && this.field.fieldOption.relationType) {
      if (this.field.fieldOption.relationType == RelationType.ManyToMany) {
        this.parseM2mControlVal();
      }
      else if (this.field.fieldOption.relationType == RelationType.ManyToOne) {
        this.parseM2oControlVal();
      }
    }

  }

  parseM2mControlVal() {
    if (this.field && this.field.fieldOption && this.field.fieldOption.relatedModelTypeCamelCase) {
      let lookUpGeneric = this.lookUps.lookUpData[this.field.fieldOption.relatedModelTypeCamelCase];
      let formVal = this.form.value;
      let keyNames = Object.keys(lookUpGeneric[0].key);
      let controlVal = formVal[this.field.fieldNameCamelCase];
      // let pkFields: ReleatedField[] = [];
      // let fkFields: ReleatedField[] = [];
      let selectedItems: any[] = [];

      // pkFields = this.field.fieldOption.releatedFields.filter(rf => rf.isParentField);
      // fkFields = this.field.fieldOption.releatedFields.filter(rf => !rf.isParentField);

      lookUpGeneric.forEach(item => {
        let propValue: any = {};
        //copy display name from generic lookup  
        propValue.displayName = item.displayName;

        //set primary key value based on primary key properties
        keyNames.forEach(keyName => {
          propValue[keyName] = item.key[keyName]
        });

        //set foreign key value based on foreign key properties
        // fkFields.forEach(fkProp => {
        //   propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.sourceFieldNameCamelCase]
        // });

        this._lookUpData.push(propValue);
      });

      //Parse control value and set displayName
      if (controlVal && controlVal.length > 0) {
        controlVal.forEach(item => {

          let masterItem = this._lookUpData.find(lookUp => {
            let isMatch = false;
            for (let i = 0; i < keyNames.length; i++) {
              let prop = keyNames[i];
              isMatch = lookUp[prop] === item[prop];
              if (isMatch)
                return isMatch;
            }
            return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
          });
          selectedItems.push(masterItem);
          // item.displayName = masterItem.displayName; //Not required, since selected items are patched directly
        });

        let patchVal: any = {};
        patchVal[this.field.fieldNameCamelCase] = selectedItems;
        this.form.patchValue(patchVal);
      }

    }
  }

  parseM2oControlVal() {
    let formVal = this.form.value;
    let lookUpGeneric = this.lookUps.lookUpData[this.field.fieldOption.relatedModelTypeCamelCase];
    let keyNames = Object.keys(lookUpGeneric[0].key);
    let controlVal = formVal[this.field.fieldNameCamelCase];

    lookUpGeneric.forEach(item => {
      let propValue: any = {};
      //copy display name from generic lookup  
      propValue.displayName = item.displayName;

      keyNames.forEach(keyName => {
        propValue[keyName] = item.key[keyName]
      });

      this._lookUpData.push(propValue);
    });


    if (controlVal) {
      let masterItem = this._lookUpData.find(lookUp => {
        let isMatch = false;
        for (let i = 0; i < keyNames.length; i++) {
          let prop = keyNames[i];
          isMatch = lookUp[prop] === controlVal[prop];
          if (isMatch)
            return isMatch;
        }
        return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
      });

      //controlVal.displayName = masterItem.displayName; //Not required, since selected item is patched directly
      let patchVal: any = {};
      patchVal[this.field.fieldNameCamelCase] = masterItem;
      this.form.patchValue(patchVal);
    }
  }

  get lookUpData() {
    return this._lookUpData;
  }

  get f() { return this.form.controls; }

  hasError(field: Field): boolean {
    return this.f[field.fieldNameCamelCase].errors && this.f[field.fieldNameCamelCase].touched;
  }

  onIsValidateChange(isValidate: boolean): void {
    let formControl = this.f[this.field.fieldNameCamelCase];
    if (isValidate && !this.isDisabled) {
      let syncValidators: ValidatorFn[] = [];
      let asyncValidators: AsyncValidatorFn[] = [];

      syncValidators.push(Validators.required);


      switch (this.field.fieldOption.validationType) {
        case ValidationType.Email:
          syncValidators.push(Validators.email);
          // syncValidators.push(Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'));
          break;
        case ValidationType.NumberOnly:
          syncValidators.push(Validators.pattern("^[0-9]*$"));
          break;
        case ValidationType.LettersOnly:
          syncValidators.push(Validators.pattern("^[a-zA-Z]*$"));
          break;
        case ValidationType.RegEx:
          syncValidators.push(Validators.pattern(this.field.fieldOption.validatorRegEx));
          break;
        case ValidationType.UserExist:
          asyncValidators.push(this.userExistValidator.validate.bind(this.userExistValidator));
          break;
        case ValidationType.UserExistByEmail:
          asyncValidators.push(this.emailExistValidator.validate.bind(this.emailExistValidator));
          break;
        case ValidationType.Password:
          asyncValidators.push(this.passwordValidator.validate.bind(this.passwordValidator));
          break;
        case ValidationType.Custom:
          this.customValidator.formType = this.formType;
          this.customValidator.formName = this.formName;
          this.customValidator.fieldName = this.field.fieldName;
          asyncValidators.push(this.customValidator.validate.bind(this.customValidator));
          break;
      }

      formControl.setValidators(syncValidators);

      if (asyncValidators.length > 0) {
        formControl.setAsyncValidators(asyncValidators);
      }
    }
    else {
      formControl.setValidators(null);

      if (formControl.asyncValidator && formControl.asyncValidator.length > 0) {
        formControl.setAsyncValidators(null);
      }

    }
    formControl.updateValueAndValidity();
  }

}
