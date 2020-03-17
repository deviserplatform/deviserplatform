import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { repeat, pairwise, startWith } from 'rxjs/operators';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
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
import { FormControlService } from '../common/services/form-control.service';
import { AdminService } from '../common/services/admin.service';

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

  private lookUpDataSubject = new BehaviorSubject<any[]>([]);
  lookUpData$ = this.lookUpDataSubject.asObservable()

  // _lookUpData: any[];
  Editor = ClassicEditor;

  private valChangeSubscription: Subscription;

  constructor(private emailExistValidator: EmailExistValidator,
    private adminService: AdminService,
    private passwordValidator: PasswordValidator,
    private userExistValidator: UserExistValidator,
    private customValidator: CustomValidator) {
    // this._lookUpData = [];


  }

  ngOnInit() {

    if (this.field && this.field.fieldOption && this.field.fieldOption.hasLookupFilter) {
      console.log(this.field.fieldOption);
      let filterFormCtrl = this.form.get(this.field.fieldOption.lookupFilterField.fieldNameCamelCase);
      this.valChangeSubscription = filterFormCtrl.valueChanges
        .pipe(startWith(null), pairwise())
        .subscribe(([prev, next]: [any, any]) => {
          let val = next ? next : prev;
          console.log(val);
          this.adminService.getLookUp(this.formType, this.formName, this.field.fieldName, val)
            .subscribe(lookupResult => {
              // console.log(lookupResult);
              this.parseControlValue(lookupResult);
            });
        });
    }
    else {
      this.parseControlValue(null);
    }
  }

  ngOnDestroy() {
    if (this.valChangeSubscription) {
      this.valChangeSubscription.unsubscribe();
    }
  }

  ngOnChanges(changes: SimpleChanges) {

    // if (changes.isValidate && changes.isValidate.currentValue != null && changes.isValidate.currentValue !== "") {
    //   //Dynamic validation can be changed only when this field is not required by backend
    //   if (!this.field.fieldOption.isRequired || this.field.fieldOption.validationType) {
    //     this.onIsValidateChange(changes.isValidate.currentValue);
    //   }
    // }

    // if (this.field.fieldOption.hasLookupFilter) {

    // }
  }

  parseControlValue(lookUpGeneric: any) {
    if (this.field && this.field.fieldOption && this.field.fieldOption.relationType) {

      if (!lookUpGeneric && (this.field.fieldOption.relationType == RelationType.ManyToMany || this.field.fieldOption.relationType == RelationType.ManyToOne)) {
        // lookUpGeneric = this.lookUps.lookUpData[this.field.fieldOption.lookupModelTypeCamelCase];
        lookUpGeneric = this.lookUps.lookUpData[this.field.fieldNameCamelCase];
      }


      if (this.field.fieldOption.relationType == RelationType.ManyToMany) {
        this.parseM2mControlVal(lookUpGeneric);
      }
      else if (this.field.fieldOption.relationType == RelationType.ManyToOne) {
        this.parseM2oControlVal(lookUpGeneric);
      }
    }

  }

  parseM2mControlVal(lookUpGeneric: any) {
    if (this.field &&
      this.field.fieldOption &&
      // this.field.fieldOption.lookupModelTypeCamelCase &&
      lookUpGeneric) {
      let formVal = this.form.value;
      let keyNames = Object.keys(lookUpGeneric[0].key);
      let controlVal = formVal[this.field.fieldNameCamelCase];
      let lookUp = [];
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

        lookUp.push(propValue);
      });

      //Parse control value and set displayName
      if (controlVal && controlVal.length > 0) {
        controlVal.forEach(item => {

          let masterItem = lookUp.find(lookUp => {
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

        setTimeout(() => {
          this.form.patchValue(patchVal);
        });


      }

      this.lookUpDataSubject.next(lookUp);
    }
  }

  parseM2oControlVal(lookUpGeneric: any) {
    if (lookUpGeneric) {
      let formVal = this.form.value;
      let keyNames = Object.keys(lookUpGeneric[0].key);
      let controlVal = formVal[this.field.fieldNameCamelCase];
      let lookUp = [];

      lookUpGeneric.forEach(item => {
        let propValue: any = {};
        //copy display name from generic lookup  
        propValue.displayName = item.displayName;

        keyNames.forEach(keyName => {
          propValue[keyName] = item.key[keyName]
        });

        lookUp.push(propValue);
      });


      if (controlVal) {
        let masterItem = lookUp.find(lookUp => {
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

        setTimeout(() => {
          this.form.patchValue(patchVal);
        });

      }

      this.lookUpDataSubject.next(lookUp);
    }
  }

  // get lookUpData() {
  //   return this._lookUpData;
  // }

  get f() { return this.form.controls; }

  hasError(field: Field): boolean {
    return this.f[field.fieldNameCamelCase].errors && this.f[field.fieldNameCamelCase].touched;
  }

  // onIsValidateChange(isValidate: boolean): void {
  //   let formControl = this.f[this.field.fieldNameCamelCase];
  //   if (isValidate && !this.isDisabled) {
  //     let syncValidators: ValidatorFn[] = [];
  //     let asyncValidators: AsyncValidatorFn[] = [];

  //     syncValidators.push(Validators.required);


  //     switch (this.field.fieldOption.validationType) {
  //       case ValidationType.Email:
  //         syncValidators.push(Validators.email);
  //         // syncValidators.push(Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'));
  //         break;
  //       case ValidationType.NumberOnly:
  //         syncValidators.push(Validators.pattern("^[0-9]*$"));
  //         break;
  //       case ValidationType.LettersOnly:
  //         syncValidators.push(Validators.pattern("^[a-zA-Z]*$"));
  //         break;
  //       case ValidationType.RegEx:
  //         syncValidators.push(Validators.pattern(this.field.fieldOption.validatorRegEx));
  //         break;
  //       case ValidationType.UserExist:
  //         asyncValidators.push(this.userExistValidator.validate.bind(this.userExistValidator));
  //         break;
  //       case ValidationType.UserExistByEmail:
  //         asyncValidators.push(this.emailExistValidator.validate.bind(this.emailExistValidator));
  //         break;
  //       case ValidationType.Password:
  //         asyncValidators.push(this.passwordValidator.validate.bind(this.passwordValidator));
  //         break;
  //       case ValidationType.Custom:
  //         this.customValidator.formType = this.formType;
  //         this.customValidator.formName = this.formName;
  //         this.customValidator.fieldName = this.field.fieldName;
  //         asyncValidators.push(this.customValidator.validate.bind(this.customValidator));
  //         break;
  //     }

  //     formControl.setValidators(syncValidators);

  //     if (asyncValidators.length > 0) {
  //       formControl.setAsyncValidators(asyncValidators);
  //     }
  //   }
  //   else {
  //     formControl.setValidators(null);

  //     if (formControl.asyncValidator && formControl.asyncValidator.length > 0) {
  //       formControl.setAsyncValidators(null);
  //     }

  //   }
  //   formControl.updateValueAndValidity({
  //     onlySelf: true,
  //     emitEvent:false    
  //   });
  // }

}
