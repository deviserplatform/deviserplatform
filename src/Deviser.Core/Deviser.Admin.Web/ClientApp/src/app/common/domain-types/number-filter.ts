import { NumberOperator } from './number-operator';
import { Filter } from './filter';

export class NumberFilter implements Filter{
    fieldName: string;
    operator: NumberOperator;
    number: number;
    fromNumber: number;
    toNumber: number;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}