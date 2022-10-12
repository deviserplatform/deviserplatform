import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FilterField } from '../../domain-types/filter-field';
import { FilterOperator } from '../../domain-types/filter-operator';
import { FilterType } from '../../domain-types/filter-type';
import { BooleanFilter } from '../../domain-types/boolean-filter';
import { FormGroup, FormControl } from '@angular/forms';
import { TextFilter } from '../../domain-types/text-filter';
import { DateTimeOperator } from '../../domain-types/date-time-operator';
import { NumberOperator } from '../../domain-types/number-operator';
import { DateFilter } from '../../domain-types/date-filter';
import { NumberFilter } from '../../domain-types/number-filter';
import { SelectFilter } from '../../domain-types/select-filter';
import { AdminConfig } from '../../domain-types/admin-config';
import { AdminService } from '../../services/admin.service';
import { FieldType } from '../../domain-types/field-type';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  @Input()
  set selectedFilter(value: FilterField) {
    this._selectedFilter = value;
    if (this._selectedFilter.field.fieldType === FieldType.Select ||
      this._selectedFilter.field.fieldType === FieldType.MultiSelect ||
      this._selectedFilter.field.fieldType === FieldType.MultiSelectCheckBox) {
      const lookUpGeneric = this.adminConfig.lookUps.lookUpData[this.selectedFilter.field.fieldNameCamelCase];
      const selectFilter = this._selectedFilter.filter as SelectFilter;
      this.lookUp = this.getLookUp(lookUpGeneric);
      this.lookUpKey = this.getLookUpKeyName(lookUpGeneric);
      // this.filterForm.patchValue({ filterKeyValues: selectFilter.filterKeyValues });
    }
  }

  get selectedFilter(): FilterField {
    return this._selectedFilter;
  }

  @Input() filterOperator: FilterOperator;

  @Output() filter = new EventEmitter<any>();
  @Output() clear = new EventEmitter<any>();

  adminConfig: AdminConfig;
  dateTimeOperator = DateTimeOperator;
  filterType = FilterType;
  filterForm: FormGroup;
  lookUp: any[];
  lookUpKey: string;
  numberOperator = NumberOperator;
  selectAll = true;
  private _selectedFilter: FilterField;


  get bsConfig(): any {
    return {
      dateInputFormat: this.selectedFilter.field.fieldOption.format.replace(/y/g, 'Y').replace(/d/g, 'D')
    };
  }

  get operators(): any[] {
    const operatorsArray = [];
    Object.entries(this.selectedFilter.operators).map((keyValue) => {
      operatorsArray.push({
        value: keyValue[0],
        label: keyValue[1]
      });
    });
    return operatorsArray;
  }

  get formValue(): any {
    return this.filterForm.value;
  }

  constructor(private _adminService: AdminService) {
    this.getAdminConfig();
  }

  getAdminConfig(): void {
    this._adminService
      .getAdminConfig()
      // .pipe(last())
      .subscribe(adminConfig => this.onGetAdminConfig(adminConfig));
  }

  onGetAdminConfig(adminConfig: AdminConfig) {
    this.adminConfig = adminConfig;
  }

  ngOnInit(): void {
    this.initForm();
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value
    const formVal = this.filterForm.value;
    switch (this.selectedFilter.filterType) {
      case FilterType.BooleanFilter:
        const booleanFilter = this.selectedFilter.filter as BooleanFilter;
        booleanFilter.isFalse = formVal.isFalse;
        booleanFilter.isTrue = formVal.isTrue;
        this.filter.emit(this.selectedFilter);
        break;
      case FilterType.DateFilter:
        const dateFilter = this.selectedFilter.filter as DateFilter;
        dateFilter.operator = formVal.operator;
        dateFilter.date = formVal.date;
        dateFilter.fromDate = formVal.fromDate;
        dateFilter.toDate = formVal.toDate;
        this.filter.emit(this.selectedFilter);
        break;
      case FilterType.NumberFilter:
        const numberFilter = this.selectedFilter.filter as NumberFilter;
        numberFilter.operator = formVal.operator;
        numberFilter.number = formVal.number;
        numberFilter.fromNumber = formVal.fromNumber;
        numberFilter.toNumber = formVal.toNumber;
        this.filter.emit(this.selectedFilter);
        break;
      case FilterType.SelectFilter:
        const selectFilter = this.selectedFilter.filter as SelectFilter;
        selectFilter.filterKeyValues = formVal.filterKeyValues;
        selectFilter.keyFieldName = this.lookUpKey;
        this.filter.emit(this.selectedFilter);
        break;
      case FilterType.TextFilter:
        const textFilter = this.selectedFilter.filter as TextFilter;
        textFilter.operator = formVal.operator;
        textFilter.text = formVal.text;
        this.filter.emit(this.selectedFilter);
        break;
    }
  }

  onClear() {
    // switch (this.selectedFilter.filterType) {
    //   case FilterType.BooleanFilter:
    //     const booleanFilter = this.selectedFilter.filter as BooleanFilter;
    //     this.filterForm.patchValue({
    //       isTrue: false,
    //       isFalse: false
    //     });
    //     break;
    // }
    this.initForm(true);
    this.clear.emit(this.selectedFilter);
  }

  onSelectFilterChange($event): void {
    console.log($event);
    const selectFilter = this.selectedFilter.filter as SelectFilter;
    selectFilter.filterKeyValues = $event;
  }

  onSelectAll($event) {
    const selectFilter = this.selectedFilter.filter as SelectFilter;
    const filterVal = $event ? this.lookUp.map(l => l[this.lookUpKey]) : [];
    this.filterForm = new FormGroup({
      filterKeyValues: new FormControl(filterVal)
    });
    this.filterForm.patchValue({ filterKeyValues: filterVal });
  }

  private initForm(isClear = false): void {
    switch (this.selectedFilter.filterType) {
      case FilterType.BooleanFilter:
        const booleanFilter = this.selectedFilter.filter as BooleanFilter;
        this.filterForm = new FormGroup({
          isTrue: new FormControl(!isClear ? booleanFilter.isTrue : false),
          isFalse: new FormControl(!isClear ? booleanFilter.isFalse : false),
        });
        break;
      case FilterType.DateFilter:
        const dateFilter = this.selectedFilter.filter as DateFilter;
        this.filterForm = new FormGroup({
          operator: new FormControl(dateFilter.operator && !isClear ? dateFilter.operator : this.operators[0].value),
          date: new FormControl(!isClear ? dateFilter.date : null),
          fromDate: new FormControl(!isClear ? dateFilter.fromDate : null),
          toDate: new FormControl(!isClear ? dateFilter.toDate : null)
        });
        break;
      case FilterType.NumberFilter:
        const numberFilter = this.selectedFilter.filter as NumberFilter;
        this.filterForm = new FormGroup({
          operator: new FormControl(numberFilter.operator && !isClear ? numberFilter.operator : this.operators[0].value),
          number: new FormControl(!isClear ? numberFilter.number : null),
          fromNumber: new FormControl(!isClear ? numberFilter.fromNumber : null),
          toNumber: new FormControl(!isClear ? numberFilter.toNumber : null)
        });
        break;
      case FilterType.SelectFilter:
        const selectFilter = this.selectedFilter.filter as SelectFilter;
        const filterVal = selectFilter.filterKeyValues && !isClear ? selectFilter.filterKeyValues : this.lookUp.map(l => l[this.lookUpKey]);
        this.filterForm = new FormGroup({
          filterKeyValues: new FormControl(filterVal)
        });
        this.filterForm.patchValue({ filterKeyValues: filterVal });
        break;
      case FilterType.TextFilter:
        const textFilter = this.selectedFilter.filter as TextFilter;
        this.filterForm = new FormGroup({
          operator: new FormControl(textFilter.operator && !isClear ? textFilter.operator : this.operators[0].value),
          text: new FormControl(!isClear ? textFilter.text : ''),
        });
        break;
    }
  }

  private getLookUp(lookUpGeneric: any): any[] {
    if (lookUpGeneric) {
      const keyNames = Object.keys(lookUpGeneric[0].key);
      const lookUp = [];

      lookUpGeneric.forEach(item => {
        const propValue: any = {};
        //copy display name from generic lookup  
        propValue.displayName = item.displayName;

        keyNames.forEach(keyName => {
          propValue[keyName] = item.key[keyName]
        });

        lookUp.push(propValue);
      });
      return lookUp;
    }
  }

  private getLookUpKeyName(lookUpGeneric: any): string {
    const keyNames = Object.keys(lookUpGeneric[0].key);
    return keyNames[0];
  }
}
