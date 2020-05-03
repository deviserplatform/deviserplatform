import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, Validator, AbstractControl, ValidationErrors, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';

@Component({
  selector: 'dev-checkbox',
  templateUrl: './dev-checkbox.component.html',
  styleUrls: ['./dev-checkbox.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DevCheckboxComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => DevCheckboxComponent),
      multi: true
    }]
})
export class DevCheckboxComponent implements OnInit, ControlValueAccessor, Validator {

  @Input() indeterminate: boolean;
  @Input() label: string;
  @Input() labelPosition: 'before' | 'after' = 'after';

  value?: boolean = null;
  indeterminateVal = false;

  constructor() {}

  ngOnInit(): void {
  }

  public onChangeFn: (val: boolean, indeterminate?: boolean) => void = () => { };
  public onValidatorChangeFn: (val: boolean, indeterminate?: boolean) => void = () => { };
  public onTouchedFn: (val: boolean, indeterminate?: boolean) => void = () => { };

  writeValue(val: boolean): void {
    // val && this.form.setValue(val, { emitEvent: false });
    if (val) {
      this.value = val;
    }
  }

  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    // isDisabled ? this.form.disable() : this.form.enable();
  }

  changeValue(event) {
    if (this.indeterminate) {
      //Three states on each click
      if (this.value) {
        this.value = false;
        this.indeterminateVal = false;
      } else if (!this.value) {
        this.value = null;
        this.indeterminateVal = true;
      }
      else {
        this.value = true;
        this.indeterminateVal = false;
      }
      this.onChangeFn(this.value, this.indeterminateVal);
      this.onTouchedFn(this.value, this.indeterminateVal);
      this.onValidatorChangeFn(this.value, this.indeterminateVal);
    }
    else {
      //Just true/false
      this.value = !this.value;
      this.onChangeFn(this.value);
      this.onTouchedFn(this.value);
      this.onValidatorChangeFn(this.value);
    }
  }

  validate(c: AbstractControl): ValidationErrors | null {
    if (!this.indeterminate && (this.value === undefined || this.value === null)) {
      return { invalidCheckBox: { valid: false, message: 'This field is required' } };
    }
    return null;
  }

  registerOnValidatorChange?(fn: () => void): void {
    this.onValidatorChangeFn = fn;
  }


}
