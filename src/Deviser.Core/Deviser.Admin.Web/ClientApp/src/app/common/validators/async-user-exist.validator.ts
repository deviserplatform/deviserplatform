import { AbstractControl, ValidationErrors, AsyncValidator } from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UserExistValidator implements AsyncValidator {
    constructor(private validationService: ValidationService) { }

    validate(
        ctrl: AbstractControl
    ): Observable<ValidationErrors | null> {
        return this.validationService.validateUserExist(ctrl.value).pipe(
            map(validationResult => (validationResult && !validationResult.succeeded ? { duplicateUser: validationResult } : null)),
            catchError(() => null)
        );
    }
}