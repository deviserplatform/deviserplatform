import { AbstractControl, ValidationErrors, AsyncValidator } from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Field } from '../domain-types/field';
import { FormType } from '../domain-types/form-type';

@Injectable({ providedIn: 'root' })
export class CustomValidator implements AsyncValidator {

    formType: FormType;
    formName: string;
    fieldName: string;

    constructor(private validationService: ValidationService) { }

    validate(
        ctrl: AbstractControl
    ): Observable<ValidationErrors | null> {
        return this.validationService.validateCustom(this.formType, this.formName, this.fieldName, ctrl.value).pipe(
            map(validationResult => (validationResult && !validationResult.succeeded ? { duplicateUser: validationResult } : null)),
            catchError(() => null)
        );
    }
}