import { ValidationType } from './validation-type';
import { FieldExpression } from './FieldExpression';
import { RelationType } from './relation-type';
import { RelatedField } from './related-field';
import { FormMode } from './form-mode';
import { LabelOption } from './label-option';
import { Field } from './field';

export interface FieldOption {    
    description: string;
    displayName: string;
    enableOn: FieldExpression;
    enableIn: FormMode;
    format: string;
    hasLookupFilter: boolean
    isHidden: boolean;
    isReadOnly: boolean;
    isRequired: boolean;
    labelOption: LabelOption;    
    maxLength: number;
    nullDisplayText: string;
    regExErrorMessage: string;
    relationType: RelationType;
    lookupFilterField: Field;
    lookupModelType: string;
    lookupModelTypeCamelCase: string;
    
    // relatedFields: RelatedField[];
    showOn: FieldExpression;
    showIn: FormMode;
    validateOn: FieldExpression;
    validationType: ValidationType;
    validatorRegEx: string;
}
