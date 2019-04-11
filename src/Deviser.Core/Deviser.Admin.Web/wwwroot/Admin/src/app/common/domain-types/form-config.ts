import { FieldConfig } from "./field-config";
import { FieldSetConfig } from "./field-set-config";
import { KeyField } from "./key-field";
import { ListConfig } from "./list-config";

export interface FormConfig {    
    fieldConfig: FieldConfig;
    fieldSetConfig: FieldSetConfig;
    keyFields: KeyField[];    
    listConfig: ListConfig;
}