import { Field } from './field';

export interface FieldSet {
    groupName: string;
    isOpen: boolean;
    cssClasses: string;
    description: string;
    fields: Field[][];
}