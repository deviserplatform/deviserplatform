import { Component, OnInit, Input, forwardRef, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS, FormGroup, FormControl, Validators, AbstractControl, ValidationErrors, Validator } from "@angular/forms";

import { ModelConfig } from '../common/domain-types/model-config';
import { Field } from '../common/domain-types/field';
import { LookUpDictionary } from '../common/domain-types/look-up-dictionary';
import { Observable, BehaviorSubject } from 'rxjs';
import { FormControlService } from '../common/services/form-control.service';
import { ConfirmDialogComponent } from '../common/components/confirm-dialog/confirm-dialog.component';
import { FormMode } from '../common/domain-types/form-mode';
import { FormContext } from '../common/domain-types/form-context';
import { ChildConfig } from '../common/domain-types/child-config';
import { FormType } from '../common/domain-types/form-type';
import { LabelType } from '../common/domain-types/label-type';
import { FieldType } from '../common/domain-types/field-type';

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

  // @Input() parentForm: FormGroup;
  // @Input() childFormContext: FormContext;
  @Input() formMode: FormMode;
  // @Input() modelConfig: ModelConfig;
  @Input() lookUps: LookUpDictionary;
  @Input() childConfig: ChildConfig;
  ViewState: typeof ViewState = ViewState;

  @ViewChild(ConfirmDialogComponent)
  private confirmDialogComponent: ConfirmDialogComponent;

  labelType = LabelType;
  childForm: FormGroup;
  childRecords: [any];
  formContext: FormContext;
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
    let childForm = this._formControlService.toFormGroupWithModelConfig(this.childConfig.modelConfig, this.formMode, {});
    let childFormName = this.childConfig.field.fieldNameCamelCase;
    this.formContext = {
      formGroup: childForm,
      formConfig: this.childConfig.modelConfig.formConfig,
      formName: childFormName,
      formTitle: childFormName,
      formType: FormType.ChildForm,
      keyField: this.childConfig.modelConfig.keyField,
      formMode: this.formMode,
      lookUps: this.lookUps
    }
    // this.childForm = this._formControlService.toFormGroupWithFormConfig(this.childFormContext.formConfig, this.childFormContext.formMode, this.childFormContext.keyField, {});
    // this.formContext.formGroup = this.childForm;
  }

  onNewItem(): void {
    this.viewState = ViewState.ADD;
    this.selectedItem = {};
    // this._childForm.next(this._formControlService.toChildFormGroup(this.formConfig, this.selectedItem));
    this.childForm = this._formControlService.toFormGroupWithModelConfig(this.childConfig.modelConfig, this.formMode, this.selectedItem);//this._formControlService.toFormGroupWithFormConfig(this.childFormContext.formConfig, this.childFormContext.formMode, this.childFormContext.keyField, this.selectedItem);
    this.formContext.formGroup = this.childForm;
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
    this.childForm = this._formControlService.toFormGroupWithModelConfig(this.childConfig.modelConfig, this.formMode, this.selectedItem);
    this.formContext.formGroup = this.childForm;
    this.viewState = ViewState.EDIT;
  }

  onUpdateItem(): void {
    let index = this.childRecords.indexOf(this.selectedItem);
    this.childRecords[index] = this.childForm.value;
    this.viewState = ViewState.LIST;
  }

  openDeleteConfirmationModal(item: any) {
    this.confirmDialogComponent.openModal(item);
  }

  onYesToDelete(item: any): void {
    console.log('confirm');
    let index = this.childRecords.indexOf(item);
    this.childRecords.splice(index, 1);
  }

  onNoToDelete(item: any): void {
    console.log('declined');
  }

  getBadge(item: any, field: Field): string {
    if (!field.fieldOption.labelOption) {
      return "";
    }

    if (!field.fieldOption.labelOption.parameters || !field.fieldOption.labelOption.parameters.paramFieldNameCamelCase) {
      if (field.fieldType == FieldType.CheckBox) {
        return item[field.fieldNameCamelCase] ? "badge-primary" : "badge-secondary";
      }
      else {
        return "badge-light";
      }
    }

    return item[field.fieldOption.labelOption.parameters.paramFieldNameCamelCase];
  }

  writeValue(obj: [any]): void {
    // throw new Error("Method not implemented.");
    this.childRecords = obj;
  }
  
  registerOnChange(fn: any): void {
    // throw new Error("Method not implemented.");
    // this.childRecordsObservable.subscribe(fn);
  }

  registerOnTouched(fn: any): void {
    // this.childRecordsObservable.subscribe(fn);
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
