import { Component, OnInit, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { Field } from '../../domain-types/field';
import { SortField } from '../../domain-types/sort-field';
import { SortState } from '../../domain-types/sort-state';
import { FilterField } from '../../domain-types/filter-field';
import { FilterType } from '../../domain-types/filter-type';
import { LookUpDictionary } from '../../domain-types/look-up-dictionary';
import { FilterOperator } from '../../domain-types/filter-operator';
import { FieldType } from '../../domain-types/field-type';
import { BooleanFilter } from '../../domain-types/boolean-filter';
import { DateFilter } from '../../domain-types/date-filter';
import { NumberFilter } from '../../domain-types/number-filter';
import { TextFilter } from '../../domain-types/text-filter';
import { SelectFilter } from '../../domain-types/select-filter';
import { AdminService } from '../../services/admin.service';
import { AdminConfig } from '../../domain-types/admin-config';
import { last } from 'rxjs/operators';

@Component({
  selector: 'thead[gridHeader]',
  templateUrl: './grid-header.component.html',
  styleUrls: ['./grid-header.component.scss']
})
export class GridHeaderComponent implements OnInit {

  // @Input() selectedField: Field;
  // @Input() lookUps: LookUpDictionary;

  @Output() filterSortChange = new EventEmitter<any>();

  adminConfig: AdminConfig;
  filters: FilterField[];
  filterOperator: FilterOperator;
  filterStyle = {};
  filterType = FilterType;
  lookUps: LookUpDictionary;
  selectedFilter: FilterField;
  sortField: SortField;
  sortState = SortState;
  private clickedOnButton = false;

  getSortCssClass(field: Field): string {

    if (!this.sortField || this.sortField.field.fieldNameCamelCase !== field.fieldNameCamelCase) {
      return '';
    }

    if (this.sortField.sortState === SortState.Ascending) {
      return 'ascending';
    } else if (this.sortField.sortState === SortState.Descending) {
      return 'descending';
    }
  }

  isFilterVisible(field: Field): boolean {
    return this.selectedFilter && this.selectedFilter.field
      && this.selectedFilter.field.fieldNameCamelCase === field.fieldNameCamelCase;
  }

  constructor(private _adminService: AdminService) {
    this.filters = [];
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
  }

  // onLabelClick() {
  //   this.sortToggle.emit(this.field);
  // }

  // toggleFilter($event: MouseEvent) {
  //   // this.isFilterVisible = !this.isFilterVisible;
  //   const filterEvent = {
  //     $event,
  //     field: this.field
  //   };
  //   this.filterToggle.emit(filterEvent);
  //   // if ($event.pageX < 200) {
  //   //   this.filterStyle = { left: 200 };
  //   // }
  // }

  onSortToggle(field: Field) {
    if (!this.sortField || this.sortField.field !== field) {
      this.sortField = {
        field,
        sortState: SortState.Ascending
      };
    }
    else {
      if (this.sortField.sortState === SortState.Ascending) {
        this.sortField.sortState = SortState.Descending;
      } else if (this.sortField.sortState === SortState.Descending) {
        this.sortField.sortState = SortState.NoSort;
      }
      else {
        this.sortField.sortState = SortState.Ascending;
      }
    }
    this.emitSortFilterEvent();
  }

  onFilterToggle(mouseEvent: MouseEvent, field: Field) {
    this.clickedOnButton = true;
    if (!this.selectedFilter || this.selectedFilter.field !== field) {
      let filterField = this.filters.find(f => f.field.fieldNameCamelCase === field.fieldNameCamelCase);
      if (!filterField) {
        filterField = this.getFilterByFieldType(field);
        this.filters.push(filterField);
      }
      if (filterField) {
        this.selectedFilter = filterField;
      }
    }
    else {
      this.selectedFilter = null;
    }
  }

  @HostListener('click')
  clickInside() {
    this.clickedOnButton = true;
  }

  @HostListener('document:click')
  onDocumentClick() {
    if (!this.clickedOnButton && this.selectedFilter) {
      this.selectedFilter = null;
    }
    this.clickedOnButton = false;
  }

  onFilter(filterField: FilterField) {
    const ff = this.filters.find(f => f.field.fieldNameCamelCase === filterField.field.fieldNameCamelCase);
    ff.isActive = true;
    if (!ff) {
      this.filters.push(ff);
    }
    this.emitSortFilterEvent();
  }

  onFilterClear(filterField: FilterField) {
    filterField.isActive = false;
    const index = this.filters.findIndex(f => f.field.fieldNameCamelCase === filterField.field.fieldNameCamelCase);
    this.emitSortFilterEvent();
  }

  private getFilterByFieldType(field: Field): FilterField | null {
    let operators: { [key: string]: string };
    switch (field.fieldType) {
      case FieldType.CheckBox:
        return { field, filterType: FilterType.BooleanFilter, filter: new BooleanFilter(field.fieldName), isActive: true };
      case FieldType.Date:
      case FieldType.DateTime:
        operators = this.adminConfig.filterOperator.dateTimeOperator;
        return { field, filterType: FilterType.DateFilter, operators, filter: new DateFilter(field.fieldName), isActive: true };
      case FieldType.Number:
      case FieldType.Currency:
        operators = this.adminConfig.filterOperator.numberOperator;
        return { field, filterType: FilterType.NumberFilter, operators, filter: new NumberFilter(field.fieldName), isActive: true };
      case FieldType.TextBox:
      case FieldType.EmailAddress:
      case FieldType.Url:
      case FieldType.RichText:
      case FieldType.Static:
        operators = this.adminConfig.filterOperator.textOperator;
        return { field, filterType: FilterType.TextFilter, operators, filter: new TextFilter(field.fieldName), isActive: true };
      case FieldType.Select:
      case FieldType.MultiSelect:
      case FieldType.MultiSelectCheckBox:
      case FieldType.RadioButton:
        return { field, filterType: FilterType.SelectFilter, filter: new SelectFilter(field.fieldName), isActive: true };
      default:
        return null;
    }
  }

  private emitSortFilterEvent() {
    const activeFilters = this.filters.filter(f => f.isActive);
    this.filterSortChange.emit({ sortField: this.sortField, filters: activeFilters });
  }
}
