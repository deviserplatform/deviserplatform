import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { Validator, ControlValueAccessor, ValidationErrors, AbstractControl, NG_VALUE_ACCESSOR, NG_VALIDATORS, FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';

@Component({
  selector: 'app-checkbox-list',
  templateUrl: './checkbox-list.component.html',
  styleUrls: ['./checkbox-list.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxListComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CheckboxListComponent),
      multi: true
    }
  ]
})
export class CheckboxListComponent implements OnInit, ControlValueAccessor, Validator {

  @Input() items: any[];
  @Input() bindLabel: string;

  internalForm: FormGroup = new FormGroup({
    checklist: new FormArray([])
  });
  initiallySelectedItems: any[];

  constructor() { }

  ngOnInit() {
  }

  public onTouched: () => void = () => { };

  writeValue(values: any[]): void {
    this.initiallySelectedItems = values;
    if (this.initiallySelectedItems.length == 0) {
      this.items.map((item, index) => {
        const control = new FormControl(false); // selected items length is zero, therefore set false to all elements by default
        (this.internalForm.controls.checklist as FormArray).push(control);
      })
    }
    else {
      this.items.map((item, index) => {
        let isItemSelected = this.initiallySelectedItems.indexOf(item) > -1;
        const control = new FormControl(isItemSelected); // selected items length is zero, therefore set false to all elements by default
        (this.internalForm.controls.checklist as FormArray).push(control);
      })
    }
  }
  registerOnChange(fn: any): void {
    this.internalForm.valueChanges.subscribe(next => {
      let checkVM: any[] = next.checklist;
      let selectedItems: any[] = [];
      checkVM.map((item, index) => {
        if (item) {
          selectedItems.push(this.items[index]);
        }
      })
      console.log(selectedItems);
      fn(selectedItems);
    });
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.internalForm.disable() : this.internalForm.enable();
  }
  validate(control: AbstractControl): ValidationErrors {
    return this.internalForm.valid ? null : { invalidForm: { valid: false, message: "checklist fields are invalid" } };
  }
}
