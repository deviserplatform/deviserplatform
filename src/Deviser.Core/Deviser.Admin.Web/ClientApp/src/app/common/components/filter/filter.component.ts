import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FilterField } from '../../domain-types/filter-field';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';
import { FilterOperator } from '../../domain-types/filter-operator';
import { FilterType } from '../../domain-types/filter-type';
import { BooleanFilter } from '../../domain-types/boolean-filter';
import { FormGroup, FormControl } from '@angular/forms';
import { ThrowStmt } from '@angular/compiler';
import { TextFilter } from '../../domain-types/text-filter';
import { DateTimeOperator } from '../../domain-types/date-time-operator';
import { NumberOperator } from '../../domain-types/number-operator';
import { DateFilter } from '../../domain-types/date-filter';
import { NumberFilter } from '../../domain-types/number-filter';
import { SelectFilter } from '../../domain-types/select-filter';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {

  @Input() selectedFilter: FilterField;
  @Input() lookUps: LookUpDictionary;
  @Input() filterOperator: FilterOperator;

  @Output() filter = new EventEmitter<any>();
  @Output() clear = new EventEmitter<any>();

  filterType = FilterType;
  filterForm: FormGroup;

  dateTimeOperator = DateTimeOperator;
  numberOperator = NumberOperator;

  constructor() { }

  get bsConfig(): any {
    return {
      dateInputFormat: this.selectedFilter.field.fieldOption.format.replace('yyyy', 'YYYY')
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

  ngOnInit(): void {
    this.resetForm();
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
    this.resetForm();
    this.clear.emit(this.selectedFilter);
  }

  private resetForm(): void {
    switch (this.selectedFilter.filterType) {
      case FilterType.BooleanFilter:
        this.filterForm = new FormGroup({
          isTrue: new FormControl(false),
          isFalse: new FormControl(false),
        });
        break;
      case FilterType.DateFilter:
        this.filterForm = new FormGroup({
          operator: new FormControl(this.operators[0].value),
          date: new FormControl(),
          fromDate: new FormControl(),
          toDate: new FormControl()
        });
        break;
      case FilterType.NumberFilter:
        this.filterForm = new FormGroup({
          operator: new FormControl(this.operators[0].value),
          number: new FormControl(),
          fromNumber: new FormControl(),
          toNumber: new FormControl()
        });
        break;
      case FilterType.SelectFilter:
        this.filterForm = new FormGroup({
          filterKeyValues: new FormControl([])
        });
        break;
      case FilterType.TextFilter:
        this.filterForm = new FormGroup({
          operator: new FormControl(this.operators[0].value),
          text: new FormControl(),
        });
        break;
    }
  }
}
