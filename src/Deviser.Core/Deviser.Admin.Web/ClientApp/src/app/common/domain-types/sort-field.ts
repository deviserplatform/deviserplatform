import { Field } from './field';
import { SortState } from './sort-state';

export interface SortField {
    field: Field;
    sortState: SortState;
}