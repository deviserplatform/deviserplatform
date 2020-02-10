import { AbstractControl, ValidationErrors, AsyncValidator } from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class PasswordValidator implements AsyncValidator {
    constructor(private validationService: ValidationService) { }

    validate(
        ctrl: AbstractControl
    ): Observable<ValidationErrors | null> {
        return this.validationService.validatePassword(ctrl.value).pipe(
            map(validationResult => (validationResult && !validationResult.succeeded ? { invalidPassword: validationResult } : null)),
            catchError(() => null)
        );
    }
}