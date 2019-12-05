import { KeyField } from "./key-field";
import { GridConfig } from "./grid-config";
import { FormConfig } from './form-config';

export interface ModelConfig {    
    formConfig:FormConfig;
    gridConfig: GridConfig;
    keyField: KeyField;
}
