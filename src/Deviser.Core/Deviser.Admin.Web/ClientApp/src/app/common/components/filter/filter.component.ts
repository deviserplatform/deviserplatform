import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FilterField } from '../../domain-types/filter-field';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';
import { FilterOperator } from '../../domain-types/filter-operator';
import { FilterType } from '../../domain-types/filter-type';
import { BooleanFilter } from '../../domain-types/boolean-filter';
import { FormGroup, FormControl } from '@angular/forms';

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

  ngOnInit(): void {
    this.filterForm = new FormGroup({
      isTrue: new FormControl(false),
      isFalse: new FormControl(false),
    });
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value
    const formVal = this.filterForm.value;
    switch (this.selectedFilter.filterType) {
      case FilterType.BooleanFilter:
        const booleanFilter = this.selectedFilter.filter as BooleanFilter;
        booleanFilter.isFalse = formVal.isFalse;
        booleanFilter.isTrue = formVal.isTrue;
        this.filter.emit(booleanFilter);
        break;
    }
  }

  onClear() {
    this.clear.emit(this.selectedFilter);
  }

}
