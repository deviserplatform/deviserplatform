import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminConfig } from '../domain-types/admin-config';


@Injectable({
  providedIn: 'root'
})
export class FormControlService {

  constructor() { }

  toFormGroup(adminConfig: AdminConfig): FormGroup {
    let group: any = {};

    if (adminConfig && adminConfig.fieldConfig &&
      adminConfig.fieldConfig.fields && adminConfig.fieldConfig.fields.length > 0) {

    }
    else if(adminConfig && adminConfig.fieldSetConfig &&
      adminConfig.fieldSetConfig.fieldSets && adminConfig.fieldSetConfig.fieldSets.length > 0){

    }

    return new FormGroup(group);
  }

}
