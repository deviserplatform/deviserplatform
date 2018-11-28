import { Field } from './field';

export interface FieldSet {
    GroupName: string;
    CssClasses: string;
    Description: string;
    Fields: Field[][];
}