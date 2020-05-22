import { KeyField } from "./key-field";
import { GridConfig } from "./grid-config";
import { FormConfig } from './form-config';
import { CustomForm } from './custom-form';
import { TreeConfig } from './tree-config';

export interface ModelConfig {
    customForms: { [key: string]: CustomForm }
    formConfig: FormConfig;
    gridConfig: GridConfig;
    keyField: KeyField;
    treeConfig: TreeConfig;
}
