import { BaseField } from './base-field';

export interface TreeConfig {
    childrenField: BaseField;
    displayField: BaseField;
    parentField: BaseField;
    sortField: BaseField;
}