import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, FormGroup, FormControl, Validators, AbstractControl, ValidationErrors, Validator } from "@angular/forms";

import { ModelConfig } from '../common/domain-types/model-config';
import { Field } from '../common/domain-types/field';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';
import { FieldType } from '../common/domain-types/field-type';

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

  @Input() form: FormGroup;
  @Input() modelConfig: ModelConfig;
  @Input() lookUps: LookUpDictionary;

  //To access FieldType enum
  fieldType = FieldType;
  childRecords: any;

  constructor() { }

  ngOnInit() {
  }

  public onTouched: () => void = () => { };
  public onChange: () => void = () => { };

  isFieldShown(field: Field) {
    if(this.hasFieldPredicate(field, 'showOn')){
      let result = this.getFieldPredicateResult(field, 'showOn');
      return result;
    }
    return true; //by default field should be visible
  }

  isFieldEnabled(field: Field) {
    if(this.hasFieldPredicate(field, 'enableOn')){
      let result = this.getFieldPredicateResult(field, 'enableOn');
      return result;
    }
    return true; //by default field should be enabled
  }

  isFieldValidate(field: Field) {
    if(this.hasFieldPredicate(field, 'validateOn')){
      let result = this.getFieldPredicateResult(field, 'validateOn');
      return result;
    }
    return false; //by default field should not be validated
  }

  hasFieldPredicate(field: Field, action: string): boolean {
    return field && field.fieldOption && field.fieldOption[action];
  }

  getFieldPredicateResult(field: Field, action: string) {
    if (field && field.fieldOption && field.fieldOption[action]) {
      let fieldExpression = field.fieldOption[action];
      let predicate = Function(...fieldExpression.parameters, fieldExpression.expression);
      let result = predicate(this.form.value);
      return result;
    }
    return false;
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
