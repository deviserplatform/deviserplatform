import { FieldType } from './field-type';
import { BaseField } from './base-field';
import { FieldOption } from './field-option';

export interface Field extends BaseField {
    fieldType: FieldType;    
    fieldOption: FieldOption;
}