import { Injectable } from '@angular/core';
import { Property } from '../domain-types/property';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  defaultWidth = 'col-md-4';

  constructor() { }

  setColumnWidth(properties: Property[]) {
    let columnWidth;

    let columnWidthProp = this.getColumnWidthProperty(properties);
    if (columnWidthProp && columnWidthProp.value) {
      let width = columnWidthProp.optionList.list.find(item => item.id === columnWidthProp.value);
      columnWidth = width.name;
    }
    else {
      columnWidth = this.defaultWidth;
    }

    return columnWidth;
  }

  getColumnWidthProperty(properties: Property[]) {
    return properties.find(prop => prop.name === 'column_width');
  }
}
