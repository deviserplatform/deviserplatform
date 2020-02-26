import { FormGroup } from '@angular/forms';
import { FormConfig } from './form-config';
import { FormMode } from './form-mode';
import { KeyField } from './key-field';
import { LookUpDictionary } from './look-up-dictionary';
import { FormType } from './form-type';

export interface FormContext {
    formGroup: FormGroup;
    formConfig: FormConfig;
    formMode: FormMode;
    formName: string;
    formTitle: string;
    formType: FormType;
    keyField: KeyField
    lookUps: LookUpDictionary;
}