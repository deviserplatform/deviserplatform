import { Component, OnInit, Input, Output, EventEmitter, forwardRef } from '@angular/core';
import { ControlValueAccessor, Validator, AbstractControl, ValidationErrors, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';

@Component({
  selector: 'dev-check-box-list',
  templateUrl: './check-box-list.component.html',
  styleUrls: ['./check-box-list.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckBoxListComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CheckBoxListComponent),
      multi: true
    }]
})
export class CheckBoxListComponent implements OnInit, ControlValueAccessor, Validator {


  @Input() dataSource: any[];
  @Input() labelField: string;
  @Input() keyField: string;
  @Input() selectedItems: any[];

  @Output() changeValue = new EventEmitter<any>();

  constructor() {

  }

  ngOnInit(): void {
  }

  public onChangeFn: (val: any[]) => void = () => { };
  public onValidatorChangeFn: (val: any[]) => void = () => { };
  public onTouchedFn: (val: any[]) => void = () => { };

  writeValue(val: any[]): void {
    this.selectedItems = val;
    this.dataSource.forEach(item => {
      if (val.indexOf(item[this.keyField]) > -1) {
        item.isChecked = true;
      }
      else {
        item.isChecked = false;
      }
    })
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

  changeCheckListValue(event, item) {
    console.log(event);
    console.log(item);
    if (!this.selectedItems) {
      this.selectedItems = [];
    }
    if (item.isChecked) {
      this.selectedItems.push(item[this.keyField]);
    }
    else {
      this.selectedItems.splice(this.selectedItems.indexOf(item[this.keyField]), 1);
    }

    this.onChangeFn(this.selectedItems);
    this.onTouchedFn(this.selectedItems);
    this.onValidatorChangeFn(this.selectedItems);
    this.changeValue.emit(this.selectedItems);
  }

  validate(c: AbstractControl): ValidationErrors | null {
    if (this.selectedItems === undefined || this.selectedItems === null) {
      return { invalidCheckBox: { valid: false, message: 'This field is required' } };
    }
    return null;
  }

  registerOnValidatorChange?(fn: () => void): void {
    this.onValidatorChangeFn = fn;
  }

}
