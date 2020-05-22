export interface FilterOperator {
    dateTimeOperator: { [key: string]: string };
    numberOperator: { [key: string]: string };
    textOperator: { [key: string]: string };
}
