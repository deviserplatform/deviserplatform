import { Pipe, PipeTransform } from '@angular/core';
import { Field } from '../domain-types/field';
import { KeyField } from '../domain-types/key-field';
import { KeyFieldType } from '../domain-types/key-field-type';

@Pipe({
  name: 'recordId'
})
export class RecordIdPipe implements PipeTransform {

  transform(record: any, keyField: KeyField): string {
    if(!keyField){
    return null
    }

    return record[keyField.fieldNameCamelCase] as string;

    
  }

}
