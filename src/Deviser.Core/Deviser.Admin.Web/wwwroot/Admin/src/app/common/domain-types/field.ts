import { FieldType } from './field-type';
import { FieldOption } from './field-option';

export interface Field {
    FieldType: FieldType;
    FieldName: string;
    FieldNameCameCase: string;
    FieldOption: FieldOption;
}