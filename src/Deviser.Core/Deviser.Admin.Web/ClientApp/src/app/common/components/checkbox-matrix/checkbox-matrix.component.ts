import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, ControlValueAccessor, Validator, AbstractControl, ValidationErrors } from '@angular/forms';
import { CheckBoxMatrix } from '../../domain-types/checkbox-matrix';

@Component({
  selector: 'app-checkbox-matrix',
  templateUrl: './checkbox-matrix.component.html',
  styleUrls: ['./checkbox-matrix.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxMatrixComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => CheckboxMatrixComponent),
      multi: true
    }
  ]
})
export class CheckboxMatrixComponent implements OnInit, ControlValueAccessor, Validator {

  @Input() rowLookUp: any[];
  @Input() colLookUp: any[];
  @Input() rowLookUpKey: string;
  @Input() colLookUpKey: string;
  @Input() checkBoxMatrix: CheckBoxMatrix;
  @Input() contextId: string;

  selectedItems: any[];
  disabled: boolean;

  constructor() { }



  ngOnInit(): void {
  }

  public onChange: (items: any[]) => void = () => { };
  public onTouched: (items: any[]) => void = () => { };

  toggle(row: any, col: any): void {
    let selectedItem = this.getSelectedItem(row, col);
    if (selectedItem) {
      //Remove
      let index = this.selectedItems.indexOf(selectedItem);
      this.selectedItems.splice(index, 1);
    } else {
      //Add
      selectedItem = {};
      selectedItem[this.checkBoxMatrix.rowKeyField.fieldNameCamelCase] = row[this.rowLookUpKey];
      selectedItem[this.checkBoxMatrix.columnKeyField.fieldNameCamelCase] = col[this.colLookUpKey];
      selectedItem[this.checkBoxMatrix.contextKeyField.fieldNameCamelCase] = this.contextId;

      this.selectedItems.push(selectedItem);
    }
    this.onChange(this.selectedItems);
    this.onTouched(this.selectedItems);
  }

  isChecked(row: any, col: any): boolean {
    return this.getSelectedItem(row, col) ? true : false;
  }

  getSelectedItem(row: any, col: any) {
    if(!this.selectedItems || this.selectedItems.length === 0) {
      return null;
    }
    let rowId = row[this.rowLookUpKey];
    let colId = col[this.colLookUpKey];
    let selectedItem = this.selectedItems.find(item => {
      return item[this.checkBoxMatrix.rowKeyField.fieldNameCamelCase] === rowId &&
      item[this.checkBoxMatrix.columnKeyField.fieldNameCamelCase] === colId &&
      item[this.checkBoxMatrix.contextKeyField.fieldNameCamelCase] === this.contextId;
    });
    return selectedItem;
  }

  writeValue(values: any[]): void {
    this.selectedItems = values;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors {
    // return this.selectedItems && this.selectedItems.length > 0 ? null :
    // { invalidForm: { valid: false, message: "checklist fields are invalid" } };
    return null;
  }
  // registerOnValidatorChange?(fn: () => void): void {
  //   throw new Error("Method not implemented.");
  // }

}
