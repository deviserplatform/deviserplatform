import { FieldType } from './field-type';
import { BaseField } from './base-field';
import { FieldOption } from './field-option';
import { BehaviorSubject, Observable } from 'rxjs';

export interface Field extends BaseField {
    fieldClrType: string;
    fieldType: FieldType;
    fieldOption: FieldOption;

    //Properties only available in UI
    isEnabledSubject: BehaviorSubject<boolean>;
    isShownSubject: BehaviorSubject<boolean>;
    isValidateSubject: BehaviorSubject<boolean>;
    
    isEnabled: Observable<boolean>;
    isShown: Observable<boolean>;
    isValidate: Observable<boolean>;

}