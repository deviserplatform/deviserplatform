import { Injectable } from '@angular/core';
import * as moment from 'moment';
@Injectable({
  providedIn: 'root'
})
export class CoreService {

  constructor() { }

  parseResponse(response) {
    this.parseRecursive(response);
    return response;
  }

  prepareRequest(obj) {
    let clonedObject = JSON.parse(JSON.stringify(obj));
    this.prepareRequestRecursive(clonedObject);

    return clonedObject;
  }

  private parseRecursive(response) {
    response.forEach((value, prop) => {

      if (response.hasOwnProperty(prop)) {

        if (moment(response[prop], moment.ISO_8601, true).isValid()) {
          response[prop] = moment(response[prop]).toDate();//new Date(response[prop]);
        }

        if (Array.isArray(response[prop]) && response[prop].length > 0) {
          response[prop].forEach(item => {
            this.parseRecursive(item);
          });
        }
      }
    });
  }

  private prepareRequestRecursive(parent) {
    parent.forEach((value, prop) => {
      if (parent.hasOwnProperty(prop)) {

        if (moment.isDate(parent[prop])) {
          parent[prop] = moment(parent[prop]).format()
        }
        
        if (Array.isArray(parent[prop]) && parent[prop].length > 0) {
          parent[prop].forEach(item => {
            this.prepareRequestRecursive(item);
          });
        }
      }
    });
  }
}
