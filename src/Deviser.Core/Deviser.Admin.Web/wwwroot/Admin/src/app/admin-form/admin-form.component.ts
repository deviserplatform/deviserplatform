import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

import { AdminService } from '../common/services/admin.service';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {
  
  metaInfo: any;
  record: any;

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

  constructor( private route: ActivatedRoute,
    private adminService: AdminService,
    private fb: FormBuilder,
    private location: Location) { }

  ngOnInit() {
    this.getMetaInfo();
    this.getRecord();
  }

  getMetaInfo(): void {
    this.adminService.getMetaInfo('Blog', 'Post')
      .subscribe(metaInfo => this.metaInfo = metaInfo);
  }


  getRecord():void {
    const id = this.route.snapshot.paramMap.get('id');
    this.adminService.getRecord('Blog', 'Post', id)
      .subscribe(record => this.record = record);
  }

  onSubmit() {
    // TODO: Use EventEmitter with form value
    console.warn(this.profileForm.value);
  }

}
