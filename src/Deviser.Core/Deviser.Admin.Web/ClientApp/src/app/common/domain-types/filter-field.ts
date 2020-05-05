import { Field } from './field';
import { Filter } from './filter';
import { FilterType as FilterType } from './filter-type';

export interface FilterField {
    field: Field;
    filterType: FilterType;
    filter?: Filter;
    isActive: boolean;
    operators?: { [key: string]: string };
}
