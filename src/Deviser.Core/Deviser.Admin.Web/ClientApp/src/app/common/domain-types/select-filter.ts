import { Filter } from './filter';

export class SelectFilter implements Filter {
    fieldName: string;
    keyFieldName: string;
    filterKeyValues: string[];
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}