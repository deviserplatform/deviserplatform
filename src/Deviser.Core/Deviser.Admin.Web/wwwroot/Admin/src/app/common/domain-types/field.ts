import { FieldType } from './field-type';
import { FieldOption } from './field-option';

export interface Field {
    fieldType: FieldType;
    fieldName: string;
    fieldNameCamelCase: string;
    fieldOption: FieldOption;
}