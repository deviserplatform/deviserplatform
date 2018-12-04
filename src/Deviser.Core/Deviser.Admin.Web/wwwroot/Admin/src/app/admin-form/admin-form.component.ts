import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';

import { AdminService } from '../common/services/admin.service';
import { AdminConfig } from '../common/domain-types/admin-config';
import { FormControlService } from '../common/services/form-control.service';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {

  metaInfo: AdminConfig;
  record: any;
  adminForm: FormGroup;

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


  getData() {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      const metaInfo$ = this.adminService.getMetaInfo('Blog', 'Post');
      const record$ = this.adminService.getRecord('Blog', 'Post', id);
      forkJoin([metaInfo$, record$]).subscribe(results => {
        this.record = results[1];
        this.onGetMetaInfo(results[0]);
      });
    } else {
      this.adminService.getMetaInfo('Blog', 'Post')
        .subscribe(metaInfo => this.onGetMetaInfo(metaInfo));
    }

  }

  // getMetaInfo(): void {
  //   this.adminService.getMetaInfo('Blog', 'Post')
  //     .subscribe(metaInfo => this.onGetMetaInfo(metaInfo));
  // }


  // getRecord(): void {
  //   const id = this.route.snapshot.paramMap.get('id');
  //   this.adminService.getRecord('Blog', 'Post', id)
  //     .subscribe(record => this.record = record);
  // }

  onGetMetaInfo(metaInfo: AdminConfig) {
    this.metaInfo = metaInfo;
    this.adminForm = this.formControlService.toFormGroup(metaInfo, this.record);
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value
    console.warn(this.profileForm.value);
  }

  goBack(): void {
    this.location.back();
  }

}
