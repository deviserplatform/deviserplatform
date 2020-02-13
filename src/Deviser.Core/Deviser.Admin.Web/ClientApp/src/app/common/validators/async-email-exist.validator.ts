import { AbstractControl, ValidationErrors, AsyncValidator } from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class EmailExistValidator implements AsyncValidator {
    constructor(private validationService: ValidationService) { }

    validate(
        ctrl: AbstractControl
    ): Observable<ValidationErrors | null> {
        return this.validationService.validateEmailExist(ctrl.value).pipe(
            map(validationResult => (validationResult && !validationResult.succeeded ? { duplicateEmail: validationResult } : null)),
            catchError(() => null)
        );
    }
}