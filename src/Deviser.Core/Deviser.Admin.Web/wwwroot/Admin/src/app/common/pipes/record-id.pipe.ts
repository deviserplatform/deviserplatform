import { Pipe, PipeTransform } from '@angular/core';
import { Field } from '../domain-types/field';

@Pipe({
  name: 'recordId'
})
export class RecordIdPipe implements PipeTransform {

  transform(record: any, keyFields: Field[]): string {
    if(keyFields){
      if(keyFields.length==1){
        let keyField = keyFields[0];
        return record[keyField.fieldNameCamelCase];
      }
      else {
        let ids = [];
        for (let keyField of keyFields){
          ids.push(record[keyField.fieldNameCamelCase]);
        }
        return ids.join(',');
      }
    }
    return null;
  }

}
