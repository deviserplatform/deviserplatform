import { Field } from './field';
import { FieldConfig } from './field-config';
import { FieldSetConfig } from './field-set-config';
import { ListConfig } from './list-config';

export interface AdminConfig {
    entityType: string;
    keyFields: Field[];
    fieldConfig: FieldConfig;
    fieldSetConfig: FieldSetConfig;
    listConfig: ListConfig;
}