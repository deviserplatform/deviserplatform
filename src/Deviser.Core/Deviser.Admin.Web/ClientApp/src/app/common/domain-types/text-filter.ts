import { TextOperator } from './text-operator';
import { Filter } from './filter';

export class TextFilter implements Filter{
    fieldName: string;
    operator: TextOperator;
    value: string;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}