import { BaseField } from "./base-field";
import { KeyFieldType } from "./key-field-type";

export interface KeyField extends BaseField{
    keyFieldType:KeyFieldType;
}