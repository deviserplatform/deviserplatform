import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { FormControlService } from '../common/services/form-control.service';
import { FormMode } from '../common/domain-types/form-mode';
import { Field } from '../common/domain-types/field';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {

  adminConfig: AdminConfig;
  record: any;
  adminForm: FormGroup;
  formMode: FormMode;

  constructor(private route: ActivatedRoute,
    private adminService: AdminService,
    private formControlService: FormControlService,
    private fb: FormBuilder,
    private location: Location) { }

  ngOnInit() {
    // this.getAdminConfig();
    // this.getRecord();
    this.getData();
  }


  getData(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.formMode = id ? FormMode.Update : FormMode.New;

    if (this.formMode === FormMode.Update) {
      const adminConfig$ = this.adminService.getAdminConfig('Blog', 'Post');
      const record$ = this.adminService.getRecord('Blog', 'Post', id);
      forkJoin([adminConfig$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetAdminConfig(results[0]);
      });
    } else if (this.formMode === FormMode.New) {
      this.adminService.getAdminConfig('Blog', 'Post')
        .subscribe(adminConfig => this.onGetAdminConfig(adminConfig));
    }

  }

  onGetAdminConfig(adminConfig: AdminConfig): void {
    this.adminConfig = adminConfig;
    this.adminForm = this.formControlService.toFormGroup(adminConfig, this.record);
  }

  onSubmit(): void {
    // TODO: Use EventEmitter with form value

    console.warn(this.adminForm.value);
    if (this.formMode === FormMode.New) {
      this.adminService.createRecord('Blog', 'Post', this.adminForm.value)
        .subscribe(formValue => this.patchFormValue(formValue));
    } else if (this.formMode === FormMode.Update) {
      this.adminService.updateRecord('Blog', 'Post', this.adminForm.value)
        .subscribe(formValue => this.patchFormValue(formValue));
    }
  }

  patchFormValue(formValue: any): void {
    this.adminForm.patchValue(formValue);
    this.goBack();
  }

  isFieldShown(field: Field) {
    let result = this.getFieldPredicate(field, 'showOn');
    return result;
  }

  isFieldEnabled(field: Field) {
    let result = this.getFieldPredicate(field, 'enableOn');
    return result;
  }

  isFieldValidate(field: Field) {
    let result = this.getFieldPredicate(field, 'validateOn');
    return result;
  }

  getFieldPredicate(field: Field, action: string) {
    if (field && field.fieldOption && field.fieldOption[action]) {
      let fieldExpression = field.fieldOption[action];
      let predicate = Function(...fieldExpression.parameters, fieldExpression.expression);
      let result = predicate(this.adminForm.value);
      return result;
    }
    return true;
  }

  goBack(): void {
    this.location.back();
  }

}
