import { Injectable } from '@angular/core';
import { Property } from '../domain-types/property';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  defaultWidth = 'col-md-4';

  constructor() { }

  getColumnWidth(properties: Property[]): string {
    let columnWidth;

    let columnWidthProp = this.getColumnWidthProperty(properties);
    if (columnWidthProp && columnWidthProp.value && columnWidthProp.optionList && columnWidthProp.optionList.list) {
      let width = columnWidthProp.optionList.list.find(item => item.id === columnWidthProp.value);
      columnWidth = width.name;
    }
    else {
      columnWidth = this.defaultWidth;
    }

    return columnWidth;
  }

  getColumnWidthProperty(properties: Property[]) {
    if (!properties || properties.length == 0) {
      return null;
    }
    return properties.find(prop => prop.name === 'column_width');    
  }

  newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = Math.random() * 16 | 0,
        v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}
