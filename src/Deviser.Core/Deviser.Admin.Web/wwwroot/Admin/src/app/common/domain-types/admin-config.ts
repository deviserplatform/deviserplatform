import { Field } from './field';
import { FieldConfig } from './field-config';
import { FieldSetConfig } from './field-set-config';
import { ListConfig } from './list-config';

export interface IAdminConfig {
    EntityType: string;
    KeyFields: Field[];
    FieldConfig: FieldConfig;
    FieldSetConfig: FieldSetConfig;
    ListConfig: ListConfig;
}