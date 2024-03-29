import { Component, OnInit, Input, forwardRef } from '@angular/core';
import {
  ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validators, AbstractControl,
  ValidationErrors, Validator, ValidatorFn, AsyncValidatorFn
} from '@angular/forms';

import { ModelConfig } from '../common/domain-types/model-config';
import { Field } from '../common/domain-types/field';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';
import { FieldType } from '../common/domain-types/field-type';
import { FormConfig } from '../common/domain-types/form-config';
import { FormMode } from '../common/domain-types/form-mode';
import { ValidationType } from '../common/domain-types/validation-type';
import { KeyField } from '../common/domain-types/key-field';
import { FormContext } from '../common/domain-types/form-context';
import { BehaviorSubject, Subject, Subscription } from 'rxjs';
import { PasswordValidator } from '../common/validators/async-password.validator';
import { UserExistValidator } from '../common/validators/async-user-exist.validator';
import { CustomValidator } from '../common/validators/async-custom.validator';
import { EmailExistValidator } from '../common/validators/async-email-exist.validator';
import { distinctUntilChanged, takeUntil, debounceTime } from 'rxjs/operators';
import { AdminService } from '../common/services/admin.service';

@Component({
  selector: 'app-entity-form',
  templateUrl: './entity-form.component.html',
  styleUrls: ['./entity-form.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => EntityFormComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => EntityFormComponent),
      multi: true
    }]
})
export class EntityFormComponent implements OnInit, ControlValueAccessor, Validator {

  // @Input() form: FormGroup;
  // @Input() formConfig: FormConfig;
  // @Input() formMode: FormMode;
  // @Input() keyField: KeyField
  // @Input() lookUps: LookUpDictionary;

  @Input() formContext: FormContext;

  //To access FieldType enum
  childRecords: any;
  fieldType = FieldType;
  formItemId: string;

  private _autoFillFields: Field[];
  private _calculatedFields: Field[];
  private _autoFieldSubscriptions: Subscription[] = [];
  private _calculatedFieldSubscriptions: Subscription[] = [];
  private _allFields: Field[];
  private _valChangeSubscription: Subscription;

  constructor(private _adminService: AdminService,
    private emailExistValidator: EmailExistValidator,
    private passwordValidator: PasswordValidator,
    private userExistValidator: UserExistValidator,
    private customValidator: CustomValidator) {
    this._allFields = [];
  }

  ngOnInit() {
    this.initUIProperties();
    this.unsubscribeValueChanges();

    this._autoFillFields = this._allFields.filter(f => f.fieldOption.autoFillField);
    this._autoFillFields.forEach(field => {
      let triggerFieldName = field.fieldOption.autoFillField.fieldNameCamelCase;
      let subscription = this.formContext.formGroup.controls[triggerFieldName].valueChanges.subscribe(value => {
        this.onAutoFillFieldValueChanges(field, value);
      });
      this._autoFieldSubscriptions.push(subscription);
    });

    this._valChangeSubscription = this.formContext.formGroup.valueChanges
      .pipe(
        // wait 300ms after each keystroke before considering the term
        debounceTime(300),
        distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b))
      )
      .subscribe(val => {
        this.onFormValueChanges(val);
      });

      this._calculatedFields = this._allFields.filter(f => f.fieldOption.calculateSelectedFields && f.fieldOption.calculateSelectedFields.length>0);
      this._calculatedFields.forEach(field => {
        let selectedFields = field.fieldOption.calculateSelectedFields;
        
        selectedFields.forEach(selectedField =>{
          let triggerFieldName = selectedField.fieldNameCamelCase;
          let subscription = this.formContext.formGroup.controls[triggerFieldName].valueChanges.subscribe(value => {
            this.onCacluatedFieldValueChanges(field, selectedFields);
          });
          this._autoFieldSubscriptions.push(subscription);
        });
      
    })
  }
  
  ngOnDestroy() {
    this.unsubscribeValueChanges();
  }



  public onTouched: () => void = () => { };
  public onChange: () => void = () => { };

  private unsubscribeValueChanges() {
    if (this._valChangeSubscription) {
      this._valChangeSubscription.unsubscribe();
    }

    if (this._autoFieldSubscriptions.length > 0) {
      this._autoFieldSubscriptions.forEach(subscription => subscription.unsubscribe());
      this._autoFieldSubscriptions = [];
    }
  }

  private initUIProperties() {

    let formConfig = this.formContext.formConfig;
    if (formConfig.fieldConfig && formConfig.fieldConfig.fields && formConfig.fieldConfig.fields.length > 0) {
      formConfig.fieldConfig.fields.forEach(fieldRow => {
        if (fieldRow.length > 0) {
          fieldRow.forEach(field => {
            this.initFieldProp(field);
          });
        }
      });
    }

    if (formConfig.fieldSetConfig && formConfig.fieldSetConfig.fieldSets && formConfig.fieldSetConfig.fieldSets.length > 0) {
      formConfig.fieldSetConfig.fieldSets.forEach(fieldSet => {
        fieldSet.isOpen = false;
        if (fieldSet.fields.length > 0) {
          fieldSet.fields.forEach(fieldRow => {
            if (fieldRow.length > 0) {
              fieldRow.forEach(field => {
                this.initFieldProp(field);
              });
            }
          });
        }
      });
      formConfig.fieldSetConfig.fieldSets[0].isOpen = true
    }
  }

  private onFormValueChanges(val: any) {
    this.formItemId = this.formContext.formGroup.value[this.formContext.keyField.fieldNameCamelCase];

    this._allFields.forEach(field => {
      let isEnabled = this.isFieldEnabled(field);
      let isShown = this.isFieldShown(field);
      let isValidate = field.fieldOption.isRequired && this.isFieldValidate(field);

      setTimeout(() => {
        field.isEnabledSubject.next(isEnabled);
      });

      setTimeout(() => {
        field.isShownSubject.next(isShown);
      });

      setTimeout(() => {
        field.isValidateSubject.next(isValidate);
      });

      setTimeout(() => {
        this.onIsValidateChange(field, isEnabled, isShown, isValidate);
      });
    });
  }

  private onAutoFillFieldValueChanges(field: Field, value: any) {

    
    value && this._adminService.autoFill(this.formContext.formType, this.formContext.formName, field.fieldName, value).subscribe(val => {      
      this.formContext.formGroup.controls[field.fieldNameCamelCase].patchValue(val.result);
      // this.formContext.formGroup.value[field.fieldNameCamelCase] = val;
    });

    // if (field.fieldOption.autoFillField) {
    //   let srcField = this._allFields.find(f => f.fieldNameCamelCase === field.fieldOption.autoFillField.fieldNameCamelCase);
    //   let srcFieldVal = this.formContext.formGroup.value[srcField.fieldNameCamelCase]
    //   srcFieldVal && this._adminService.autoFill(field.fieldName, srcFieldVal).subscribe(val => {
    //     let valToPatch = {};
    //     valToPatch[field.fieldNameCamelCase] = val.result;
    //     this.formContext.formGroup.patchValue(valToPatch);
    //     // this.formContext.formGroup.value[field.fieldNameCamelCase] = val;
    //   });
    // }
  }
  private onCacluatedFieldValueChanges( calculatedField: Field, selectedFields: Field[]) {
    let fieldAndValues:any = {};
    selectedFields.forEach(field=>{
      fieldAndValues[field.fieldName] = this.formContext.formGroup.get(field.fieldNameCamelCase).value;
    })
    fieldAndValues && this._adminService.calculate(this.formContext.formType, this.formContext.formName, calculatedField.fieldName, fieldAndValues).subscribe(val => {      
      this.formContext.formGroup.controls[calculatedField.fieldNameCamelCase].patchValue(val.result);
      // this.formContext.formGroup.value[field.fieldNameCamelCase] = val;
    });
  }

  private initFieldProp(field: Field) {
    field.isEnabledSubject = new BehaviorSubject<boolean>(true);
    field.isEnabled = field.isEnabledSubject.asObservable();
    field.isShownSubject = new BehaviorSubject<boolean>(true);
    field.isShown = field.isShownSubject.asObservable();
    field.isValidateSubject = new BehaviorSubject<boolean>(true);
    field.isValidate = field.isValidateSubject.asObservable();
    this._allFields.push(field);
  }

  private isFieldShown(field: Field) {
    if (this.hasFieldPredicate(field, 'showOn')) {
      const result = this.getFieldPredicateResult(field, 'showOn');
      return result;
    }

    return field.fieldOption.showIn == FormMode.Both || field.fieldOption.showIn == this.formContext.formMode;
    //return true; //by default field should be visible
  }

  private isFieldEnabled(field: Field) {
    if (this.hasFieldPredicate(field, 'enableOn')) {
      const result = this.getFieldPredicateResult(field, 'enableOn');
      return result;
    }
    return field.fieldOption.enableIn == FormMode.Both || field.fieldOption.enableIn == this.formContext.formMode;
    //return true; //by default field should be enabled
  }

  private isFieldValidate(field: Field) {
    if (this.hasFieldPredicate(field, 'validateOn')) {
      const result = this.getFieldPredicateResult(field, 'validateOn');
      return result;
    }

    return field.fieldOption.validationType ? true : false;

    //return false; //by default field should not be validated
  }

  private hasFieldPredicate(field: Field, action: string): boolean {
    return field && field.fieldOption && field.fieldOption[action];
  }

  private getFieldPredicateResult(field: Field, action: string) {
    var result = false;
    if (field && field.fieldOption && field.fieldOption[action]) {
      try {
        var fieldExpression = field.fieldOption[action];
        var predicate = Function(...fieldExpression.parameters, fieldExpression.expression);
        result = predicate(this.formContext.formGroup.value);
        return result;
      } catch (err) {
        console.log(err);
      }
    }
    return result;
  }

  private onIsValidateChange(field: Field, isEnabled: boolean, isShown: boolean, isValidate: boolean): void {
    let formControl = this.formContext.formGroup.get(field.fieldNameCamelCase);
    if (isValidate && isEnabled && isShown) {
      let syncValidators: ValidatorFn[] = [];
      let asyncValidators: AsyncValidatorFn[] = [];

      syncValidators.push(Validators.required);


      switch (field.fieldOption.validationType) {
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
          syncValidators.push(Validators.pattern(field.fieldOption.validatorRegEx));
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
          this.customValidator.formType = this.formContext.formType;
          this.customValidator.formName = this.formContext.formName;
          this.customValidator.fieldName = field.fieldName;
          asyncValidators.push(this.customValidator.validate.bind(this.customValidator));
          break;
      }

      formControl.setValidators(syncValidators);

      if (asyncValidators.length > 0) {
        formControl.setAsyncValidators(asyncValidators);
      }
    }
    else {
      // formControl.setValidators(null);
      formControl.clearValidators();

      if (formControl.asyncValidator && formControl.asyncValidator.length > 0) {
        // formControl.setAsyncValidators(null);
        formControl.clearAsyncValidators();
      }

    }
    formControl.updateValueAndValidity({
      onlySelf: true,
      emitEvent: false
    });
  }

  writeValue(val: any): void {
    // val && this.form.setValue(val, { emitEvent: false });
    if (val) {
      this.childRecords = val;
    }
  }

  registerOnChange(fn: any): void {
    console.log("on change");
    // this.form.valueChanges.subscribe(fn);
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    console.log("on blur");
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // isDisabled ? this.form.disable() : this.form.enable();
  }

  validate(c: AbstractControl): ValidationErrors | null {
    console.log("Basic Info validation", c);
    // return this.form.valid ? null : { invalidForm: { valid: false, message: "basicInfoForm fields are invalid" } };
    return null;
  }

}