import { Component, OnInit, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, FormGroup, FormControl, Validators, AbstractControl, ValidationErrors, Validator } from "@angular/forms";

import { FormConfig } from '../common/domain-types/form-config';
import { Field } from '../common/domain-types/field';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';
import { Observable, BehaviorSubject } from 'rxjs';
import { FormControlService } from '../common/services/form-control.service';

@Component({
  selector: 'app-child-grid',
  templateUrl: './child-grid.component.html',
  styleUrls: ['./child-grid.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ChildGridComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => ChildGridComponent),
      multi: true
    }
  ]
})
export class ChildGridComponent implements OnInit, ControlValueAccessor, Validator {


  @Input() parentForm: FormGroup;
  @Input() formConfig: FormConfig;
  @Input() lookUps: LookUpDictionary;
  ViewState: typeof ViewState = ViewState;

  childForm: FormGroup;
  childRecords: [any];
  selectedItem: any;

  viewState: ViewState;

  // _childForm: BehaviorSubject<FormGroup | undefined>;

  childRecordsObservable = new Observable((observer) => {
    // observable execution
    observer.next(this.childRecords);
    observer.complete();
  });


  constructor(private _formControlService: FormControlService) {
    this.viewState = ViewState.LIST;
  }

  ngOnInit() {
    // this._childForm = new BehaviorSubject(this._formControlService.toChildFormGroup(this.formConfig, {}));
    // this._childForm.subscribe(childForm => this.childForm = childForm);
    this.childForm = this._formControlService.toChildFormGroup(this.formConfig, {});
  }

  onNewItem(): void {
    this.viewState = ViewState.ADD;
    this.selectedItem = {};
    // this._childForm.next(this._formControlService.toChildFormGroup(this.formConfig, this.selectedItem));
    this.childForm = this._formControlService.toChildFormGroup(this.formConfig, this.selectedItem);
  }

  onAddItem(): void {
    this.selectedItem = this.childForm.value;
    this.childRecords.push(this.selectedItem);
    this.viewState = ViewState.LIST;
  }

  onCancelItem(): void {
    this.viewState = ViewState.LIST;
  }

  onEditItem(item): void {
    this.selectedItem = item;
    this.childForm = this._formControlService.toChildFormGroup(this.formConfig, this.selectedItem);
    this.viewState = ViewState.EDIT;
  }

  onUpdateItem(): void {
    let index = this.childRecords.indexOf(this.selectedItem);
    this.childRecords[index] = this.childForm.value;
    this.viewState = ViewState.LIST;
  }

  writeValue(obj: [any]): void {
    // throw new Error("Method not implemented.");
    this.childRecords = obj;
  }
  registerOnChange(fn: any): void {
    // throw new Error("Method not implemented.");
    this.childRecordsObservable.subscribe(fn);
  }
  registerOnTouched(fn: any): void {
    this.childRecordsObservable.subscribe(fn);
  }
  setDisabledState?(isDisabled: boolean): void {
    // isDisabled ? this.addressInfoForm.disable() : this.addressInfoForm.enable();
  }

  validate(c: AbstractControl): ValidationErrors | null {
    console.log("Basic Info validation", c);
    //return this.addressInfoForm.valid ? null : { invalidForm: {valid: false, message: "basicInfoForm fields are invalid"}};
    return null;
  }

}

enum ViewState {
  LIST = 'LIST',
  EDIT = 'EDIT',
  ADD = 'ADD'
};
