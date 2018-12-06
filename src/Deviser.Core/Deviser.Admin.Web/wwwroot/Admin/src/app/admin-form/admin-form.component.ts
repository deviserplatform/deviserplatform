import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { FormControlService } from '../common/services/form-control.service';
import { FormMode } from '../common/domain-types/form-mode';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {

  metaInfo: AdminConfig;
  record: any;
  adminForm: FormGroup;
  formMode: FormMode;
  // adminForm = this.fb.group({
  //   fieldss: this.fb.group({
  //     title: [''],
  //     content: ['']
  //   }),
  // });

  profileForm = this.fb.group({
    firstName: [''],
    lastName: [''],
    address: this.fb.group({
      street: [''],
      city: [''],
      state: [''],
      zip: ['']
    }),
  });



  constructor(private route: ActivatedRoute,
    private adminService: AdminService,
    private formControlService: FormControlService,
    private fb: FormBuilder,
    private location: Location) { }

  ngOnInit() {
    // this.getMetaInfo();
    // this.getRecord();
    this.getData();
  }


  getData(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.formMode = id ? FormMode.Update : FormMode.New;

    if (this.formMode === FormMode.Update) {
      const metaInfo$ = this.adminService.getMetaInfo('Blog', 'Post');
      const record$ = this.adminService.getRecord('Blog', 'Post', id);
      forkJoin([metaInfo$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetMetaInfo(results[0]);
      });
    } else if (this.formMode === FormMode.New) {
      this.adminService.getMetaInfo('Blog', 'Post')
        .subscribe(metaInfo => this.onGetMetaInfo(metaInfo));
    }

  }

  onGetMetaInfo(metaInfo: AdminConfig): void {
    this.metaInfo = metaInfo;
    this.adminForm = this.formControlService.toFormGroup(metaInfo, this.record);
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

  goBack(): void {
    this.location.back();
  }

}
