import { KeyField } from './key-field';

export interface CheckBoxMatrix {
    rowType: string;
    rowTypeCamelCase: string;
    columnType: string;
    columnTypeCamelCase: string;
    matrixKeyField: KeyField;
    rowKeyField: KeyField;
    columnKeyField: KeyField;
    contextKeyField: KeyField;
}
