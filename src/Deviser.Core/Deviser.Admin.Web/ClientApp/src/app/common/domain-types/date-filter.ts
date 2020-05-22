import { Filter } from './filter';
import { DateTimeOperator } from './date-time-operator';

export class DateFilter implements Filter {
    fieldName: string;
    operator: DateTimeOperator;
    date: Date;
    fromDate: Date;
    toDate: Date;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}