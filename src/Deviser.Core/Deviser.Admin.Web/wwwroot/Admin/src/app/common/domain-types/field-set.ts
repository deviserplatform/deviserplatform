import { Field } from './field';

export interface FieldSet {
    groupName: string;
    cssClasses: string;
    description: string;
    fields: Field[][];
}