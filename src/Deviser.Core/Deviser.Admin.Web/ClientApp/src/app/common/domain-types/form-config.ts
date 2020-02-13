import { FieldConfig } from "./field-config";
import { FieldSetConfig } from "./field-set-config";
import { FormOption } from './form-option';
import { AdminAction } from './admin-action';

export interface FormConfig {
    fieldConfig: FieldConfig;
    fieldSetConfig: FieldSetConfig;
    formActions: { [key: string]: AdminAction };
    formOption: FormOption;
}
