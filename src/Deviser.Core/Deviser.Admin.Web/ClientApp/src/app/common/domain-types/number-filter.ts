import { NumberOperators } from './number-operator';
import { Filter } from './filter';

export class NumberFilter implements Filter{
    fieldName: string;
    numberOperators: NumberOperators;
    value: number;
    from: number;
    to: number;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}