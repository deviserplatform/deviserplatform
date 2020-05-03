import { Filter } from './filter';

export class BooleanFilter implements Filter {
    fieldName: string;
    isTrue: boolean;
    isFalse: boolean;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
        this.isTrue = false;
        this.isFalse = false;
    }
}
