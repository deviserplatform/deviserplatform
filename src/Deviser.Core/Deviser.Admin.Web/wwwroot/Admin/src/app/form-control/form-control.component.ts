import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';
import { repeat } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ValidationType } from '../common/domain-types/validation-type';
import { EmailExistValidator } from '../common/validators/async-email-exist.validator';
import { PasswordValidator } from '../common/validators/async-password.validator';
import { UserExistValidator } from '../common/validators/async-user-exist.validator';

@Component({
  selector: 'app-form-control',
  templateUrl: './form-control.component.html',
  styleUrls: ['./form-control.component.scss']
})
export class FormControlComponent implements OnInit {

  @Input() field: Field;
  @Input() form: FormGroup;

  @Input() isShown: boolean;
  @Input() isDisabled: boolean;
  @Input() isValidate: boolean;

  fieldType = FieldType;

  constructor(private emailExistValidator: EmailExistValidator,
    private passwordValidator: PasswordValidator,
    private userExistValidator: UserExistValidator) { }

  ngOnInit() {

  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes.isValidate && changes.isValidate.currentValue != null) {
      //Dynamic validation can be changed only when this field is not required by backend
      if (!this.field.fieldOption.isRequired) {
        this.onIsValidateChange(changes.isValidate.currentValue);
      }
    }
  }



  get f() { return this.form.controls; }

  hasError(field: Field): boolean {
    return this.f[field.fieldNameCamelCase].errors && this.f[field.fieldNameCamelCase].touched;
  }

  hasRequiredError(field: Field): boolean {
    return this.f[field.fieldNameCamelCase].errors.required;
  }

  onIsValidateChange(isValidate: boolean): void {
    let formControl = this.f[this.field.fieldNameCamelCase];
    if (isValidate) {
      let syncValidators: ValidatorFn[] = [];
      let asyncValidators: AsyncValidatorFn[] = [];

      syncValidators.push(Validators.required);


      switch (this.field.fieldOption.validationType) {
        case ValidationType.Email:
          syncValidators.push(Validators.email)
          break;
        case ValidationType.NumberOnly:
          syncValidators.push(Validators.pattern("^[0-9]*$"))
          break;
        case ValidationType.LettersOnly:
          syncValidators.push(Validators.pattern("^[a-zA-Z]*$"))
          break;
        case ValidationType.RegEx:
          syncValidators.push(Validators.pattern(this.field.fieldOption.validatorRegEx))
          break;
        case ValidationType.UserExist:
          asyncValidators.push(this.userExistValidator.validate.bind(this.userExistValidator));
          break;
        case ValidationType.UserExistByEmail:
          asyncValidators.push(this.emailExistValidator.validate.bind(this.emailExistValidator));
          break;
        case ValidationType.Password:
          asyncValidators.push(this.passwordValidator.validate.bind(this.passwordValidator));
          break;
      }

      formControl.setValidators([Validators.required]);

      if (asyncValidators.length > 0) {
        formControl.setAsyncValidators(asyncValidators);
      }
    }
    else {
      formControl.setValidators(null);

      if (formControl.asyncValidator.length > 0) {
        formControl.setAsyncValidators(null);
      }

    }
    formControl.updateValueAndValidity();
  }

}
