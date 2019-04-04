import { ValidationType } from './validation-type';
import { FieldExpression } from './FieldExpression';
import { RelationType } from './relation-type';
import { ReleatedField } from './releated-field';

export interface FieldOption {
    enableOn: FieldExpression;
    description: string;
    displayName: string;    
    format: string;
    releatedFields: ReleatedField[];
    isHidden: boolean;
    isReadOnly: boolean;
    isRequired: boolean;    
    maxLength: number; 
    nullDisplayText: string;
    regExErrorMessage: string;
    relationType: RelationType;
    releatedEntityType: string;
    releatedEntityTypeCamelCase: string;
    showOn: FieldExpression;
    validateOn: FieldExpression;
    validationType: ValidationType;
    validatorRegEx: string;
}