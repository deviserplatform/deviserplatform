import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
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

  //To access FieldType enum
  fieldType = FieldType;
  _lookUpData: any[];
  Editor = ClassicEditor;

  constructor(private emailExistValidator: EmailExistValidator,
    private passwordValidator: PasswordValidator,
    private userExistValidator: UserExistValidator) {
    this._lookUpData = [];


  }

  ngOnInit() {
    if (this.field && this.field.fieldOption && this.field.fieldOption.releatedEntityTypeCamelCase &&
      this.field.fieldOption.foreignKeyFields) {
      let lookUpGeneric = this.lookUps.lookUpData[this.field.fieldOption.releatedEntityTypeCamelCase];
      let formVal = this.form.value;
      let controlVal = formVal[this.field.fieldNameCamelCase];
      let pkProperties = [];
      let fkProperties = [];

      this.field.fieldOption.foreignKeyFields.forEach(fkField => {
        pkProperties = pkProperties.concat(fkField.properties.filter(prop => prop.isPKProperty));
        fkProperties = fkProperties.concat(fkField.properties.filter(prop => !prop.isPKProperty));
      });

      lookUpGeneric.forEach(item => {
        let propValue: any = {};
        //copy display name from generic lookup  
        propValue.displayName = item.displayName;

        //set primary key value based on primary key properties
        pkProperties.forEach(pkProp => {
          propValue[pkProp.fieldNameCamelCase] = formVal[pkProp.principalFieldNameCamelCase]
        });

        //set foreign key value based on foreign key properties
        fkProperties.forEach(fkProp => {
          propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
        });

        this._lookUpData.push(propValue);
      });

      //Parse control value and set displayName
      if (controlVal && controlVal.length > 0) {
        controlVal.forEach(item => {

          let masterItem = this._lookUpData.find(lookUp => {
            let isMatch = false;
            for (let i = 0; i < fkProperties.length; i++) {
              let prop = fkProperties[i];
              isMatch = lookUp[prop.fieldNameCamelCase] === item[prop.fieldNameCamelCase];
              if (isMatch)
                return isMatch;
            }
            return false; // propValue[fkProp.fieldNameCamelCase] = item.key[fkProp.principalFieldNameCamelCase]
          });

          item.displayName = masterItem.displayName
        });
      }

    }
  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes.isValidate && changes.isValidate.currentValue != null) {
      //Dynamic validation can be changed only when this field is not required by backend
      if (!this.field.fieldOption.isRequired) {
        this.onIsValidateChange(changes.isValidate.currentValue);
      }
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
    if (isValidate) {
      let syncValidators: ValidatorFn[] = [];
      let asyncValidators: AsyncValidatorFn[] = [];

      syncValidators.push(Validators.required);


      switch (this.field.fieldOption.validationType) {
        case ValidationType.Email:
          syncValidators.push(Validators.email)
          break;
        case ValidationType.NumberOnly:
          syncValidators.push(Validators.pattern("^[0-9]*$"))
          break;
        case ValidationType.LettersOnly:
          syncValidators.push(Validators.pattern("^[a-zA-Z]*$"))
          break;
        case ValidationType.RegEx:
          syncValidators.push(Validators.pattern(this.field.fieldOption.validatorRegEx))
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
      }

      formControl.setValidators([Validators.required]);

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
