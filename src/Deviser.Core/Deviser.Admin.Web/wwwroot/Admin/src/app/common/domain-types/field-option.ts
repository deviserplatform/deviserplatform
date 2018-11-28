import { ValidationType } from './validation-type';

export interface FieldOption {
    DisplayName: string;
    Description: string;
    Format: string;
    MaxLength: number;
    NullDisplayText: string;
    IsHidden: boolean;
    IsReadOnly: boolean;
    IsRequired: boolean;
    ShowOn: string;
    EnableOn: string;
    ValidateOn: string;
    ValidationType: ValidationType;
    ValidatorRegEx: string;
}