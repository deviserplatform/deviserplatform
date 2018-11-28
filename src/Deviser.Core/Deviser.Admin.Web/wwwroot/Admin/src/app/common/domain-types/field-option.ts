import { ValidationType } from './validation-type';

export interface FieldOption {
    displayName: string;
    description: string;
    format: string;
    maxLength: number;
    nullDisplayText: string;
    isHidden: boolean;
    isReadOnly: boolean;
    isRequired: boolean;
    showOn: string;
    enableOn: string;
    validateOn: string;
    validationType: ValidationType;
    validatorRegEx: string;
}