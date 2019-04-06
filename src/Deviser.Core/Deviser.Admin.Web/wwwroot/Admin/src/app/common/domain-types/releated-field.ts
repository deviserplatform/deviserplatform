import { BaseField } from "./base-field";

export interface ReleatedField extends BaseField {    
    fieldClrType: string;
    isParentField: boolean;
    sourceClrType: string;
    sourceFieldName: string;
    sourceFieldNameCamelCase: string
}