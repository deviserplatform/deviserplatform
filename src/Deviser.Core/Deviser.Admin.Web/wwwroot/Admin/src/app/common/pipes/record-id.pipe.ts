import { Pipe, PipeTransform } from '@angular/core';
import { Field } from '../domain-types/field';
import { KeyField } from '../domain-types/key-field';
import { KeyFieldType } from '../domain-types/key-field-type';

@Pipe({
  name: 'recordId'
})
export class RecordIdPipe implements PipeTransform {

  transform(record: any, keyFields: KeyField[]): string {
    if (keyFields) {
      let pkFields:KeyField[] = keyFields.filter(f=> f.keyFieldType === KeyFieldType.PrimaryKey);

      if (pkFields.length === 1) {
        const keyField = pkFields[0];
        return record[keyField.fieldNameCamelCase];
      } else {
        const ids = [];
        for (const keyField of pkFields) {
          ids.push(record[keyField.fieldNameCamelCase]);
        }
        return ids.join(',');
      }
    }
    return null;
  }

}
