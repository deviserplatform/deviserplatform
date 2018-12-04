import { Pipe, PipeTransform } from '@angular/core';
import { Field } from '../domain-types/field';
import { DatePipe } from '@angular/common';
import { FieldType } from '../domain-types/field-type';

@Pipe({
  name: 'formatField'
})
export class FormatFieldPipe implements PipeTransform {

  constructor(private datePipe: DatePipe) { }

  transform(value: any, field: Field): string {
    if (field && field.fieldOption && field.fieldOption.format) {
      if (field.fieldType === FieldType.DateTime) {
        return this.datePipe.transform(value, field.fieldOption.format);
      }
    }
    return value;
  }

}
