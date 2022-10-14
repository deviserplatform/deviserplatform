import { Injectable } from '@angular/core';
import * as moment from 'moment';
@Injectable({
  providedIn: 'root'
})
export class CoreService {

  constructor() { }

  parseResponse(response) {
    if (!response) return;

    this.parseRecursive(response);
    return response;
  }

  prepareRequest(obj) {
    let clonedObject = JSON.parse(JSON.stringify(obj));
    this.prepareRequestRecursive(clonedObject);

    return clonedObject;
  }

  private parseRecursive(response) {

    Object.keys(response).forEach(key => {
      let value = response[key];

      if (response.hasOwnProperty(key)) {

        let value = response[key];
        const newLocal = value instanceof String;
        if(!value || Array.isArray(value) || 
        typeof value !== 'string' || !(newLocal) 
        || !moment(value).isValid()) return;

        let momentObj = moment(value, moment.ISO_8601, true);
        if (momentObj && momentObj.isValid()) {
          response[key] = moment(response[key]).toDate();//new Date(response[prop]);
        }

        if (Array.isArray(response[key]) && response[key].length > 0) {
          response[key].forEach(item => {
            this.parseRecursive(item);
          });
        }
      }

    });
  }

  private prepareRequestRecursive(parent) {
    Object.keys(parent).forEach(key => {
      let value = parent[key];
      if (parent.hasOwnProperty(key)) {

        if (moment.isDate(parent[key])) {
          parent[key] = moment(parent[key]).format()
        }

        if (Array.isArray(parent[key]) && parent[key].length > 0) {
          parent[key].forEach(item => {
            this.prepareRequestRecursive(item);
          });
        }
      }
    });
  }
}
