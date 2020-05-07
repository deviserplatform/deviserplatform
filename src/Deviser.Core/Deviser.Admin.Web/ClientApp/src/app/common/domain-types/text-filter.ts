import { TextOperator } from './text-operator';
import { Filter } from './filter';

export class TextFilter implements Filter{
    fieldName: string;
    operator: TextOperator;
    text: string;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}