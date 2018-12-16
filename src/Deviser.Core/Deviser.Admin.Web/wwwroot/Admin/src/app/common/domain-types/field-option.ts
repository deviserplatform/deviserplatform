import { ValidationType } from './validation-type';
import { FieldExpression } from './FieldExpression';

export interface FieldOption {
    displayName: string;
    description: string;
    format: string;
    maxLength: number;
    nullDisplayText: string;
    isHidden: boolean;
    isReadOnly: boolean;
    isRequired: boolean;
    showOn: FieldExpression;
    enableOn: FieldExpression;
    validateOn: FieldExpression;
    validationType: ValidationType;
    validatorRegEx: string;
}