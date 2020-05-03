import { Filter } from './filter';
import { DateTimeOperator } from './date-time-operator';

export class DateFilter implements Filter {
    fieldName: string;
    dateTimeOperator: DateTimeOperator;
    value: Date;
    from: Date;
    to: Date;
    constructor(fieldName: string) {
        this.fieldName = fieldName;
    }
}