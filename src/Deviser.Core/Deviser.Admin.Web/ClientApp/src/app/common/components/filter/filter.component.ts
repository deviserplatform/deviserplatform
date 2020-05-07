import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FilterField } from '../../domain-types/filter-field';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';
import { FilterOperator } from '../../domain-types/filter-operator';
import { FilterType } from '../../domain-types/filter-type';
import { BooleanFilter } from '../../domain-types/boolean-filter';
import { FormGroup, FormControl } from '@angular/forms';
import { ThrowStmt } from '@angular/compiler';
import { TextFilter } from '../../domain-types/text-filter';

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

  constructor() { }

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
      case FilterType.TextFilter:
        this.filterForm = new FormGroup({
          operator: new FormControl(this.operators[0].value),
          text: new FormControl(),
        });
        break;
    }
  }

}
