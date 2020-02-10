import { ValidationType } from './validation-type';
import { FieldExpression } from './FieldExpression';
import { RelationType } from './relation-type';
import { RelatedField } from './related-field';
import { FormMode } from './form-mode';

export interface FieldOption {    
    enableOn: FieldExpression;
    description: string;
    displayName: string;
    format: string;
    relatedFields: RelatedField[];
    showIn: FormMode;
    isHidden: boolean;
    isReadOnly: boolean;
    isRequired: boolean;
    maxLength: number;
    nullDisplayText: string;
    regExErrorMessage: string;
    relationType: RelationType;
    relatedModelType: string;
    relatedModelTypeCamelCase: string;
    showOn: FieldExpression;
    validateOn: FieldExpression;
    validationType: ValidationType;
    validatorRegEx: string;
}
 