import { FormConfig } from './form-config';
import { KeyField } from './key-field';

export interface CustomForm {
    formName:string;
    formConfig:FormConfig    
    keyField:KeyField;
    modelType:string;
}