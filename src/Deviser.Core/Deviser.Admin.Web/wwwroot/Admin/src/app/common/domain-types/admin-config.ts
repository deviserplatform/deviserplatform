import { Field } from './field';
import { FieldConfig } from './field-config';
import { FieldSetConfig } from './field-set-config';
import { ListConfig } from './list-config';
import { LookUpDictionary } from './look-up-dictionary';
import { KeyField } from './key-field';

export interface AdminConfig {
    childConfigs: AdminConfig[];
    entityType: string;
    fieldConfig: FieldConfig;
    fieldSetConfig: FieldSetConfig;
    keyFields: KeyField[];    
    listConfig: ListConfig;
    LookUps: LookUpDictionary
}