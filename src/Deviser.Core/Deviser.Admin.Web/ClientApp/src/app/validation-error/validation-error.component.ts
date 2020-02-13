import { Component, OnInit, Input } from '@angular/core';
import { Field } from '../common/domain-types/field';
import { FormGroup } from '@angular/forms';
import { ValidationType } from '../common/domain-types/validation-type';

@Component({
  selector: 'app-validation-error',
  templateUrl: './validation-error.component.html',
  styleUrls: ['./validation-error.component.scss']
})
export class ValidationErrorComponent implements OnInit {

  @Input() field: Field;
  @Input() form: FormGroup;

  validationType = ValidationType;

  constructor() { }

  get f() { return this.form.controls; }

  get errors(){
    return this.f[this.field.fieldNameCamelCase].errors;
  }

  ngOnInit() {
    
  }

}
