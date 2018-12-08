import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Field } from '../common/domain-types/field';
import { FieldType } from '../common/domain-types/field-type';

@Component({
  selector: 'app-form-control',
  templateUrl: './form-control.component.html',
  styleUrls: ['./form-control.component.scss']
})
export class FormControlComponent implements OnInit {

  @Input() field: Field;
  @Input() form: FormGroup;

  fieldType = FieldType;

  constructor() { }

  ngOnInit() {
  }

  get f() { return this.form.controls; }

  hasError(field:Field): boolean{
    return this.f[field.fieldNameCamelCase].errors && this.f[field.fieldNameCamelCase].touched;
  }
 
  hasRequiredError(field: Field):boolean{
    return this.f[field.fieldNameCamelCase].errors.required;
  }

}
