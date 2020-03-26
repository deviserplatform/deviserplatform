import { Field } from './field';
import { AdminAction } from './admin-action';

export interface GridConfig {
    fields: Field[][];
    isDeleteVisible: boolean;
    isEditVisible: boolean;
    rowActions: { [key: string]: AdminAction };
}