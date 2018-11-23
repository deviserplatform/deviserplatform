import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'recordId'
})
export class RecordIdPipe implements PipeTransform {

  transform(record: any, keyFields: [any]): string {
    if(keyFields){
      if(keyFields.length==1){
        let keyField = keyFields[0];
        return record[keyField.fieldNameCameCase];
      }
      else {
        let ids = [];
        for (let keyField of keyFields){
          ids.push(record[keyField.fieldNameCameCase]);
        }
        return ids.join(',');
      }
    }
    return null;
  }

}
