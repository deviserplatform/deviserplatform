import { Field } from './field';
import { FieldConfig } from './field-config';
import { FieldSetConfig } from './field-set-config';
import { ListConfig } from './list-config';
import { LookUpDictionary } from './look-up-dictionary';
import { KeyField } from './key-field';
import { FormConfig } from './form-config';
import { ChildConfig } from './child-config';

export interface AdminConfig {
    childConfigs: ChildConfig[];
    entityType: string;
    formConfig: FormConfig;
    LookUps: LookUpDictionary
}