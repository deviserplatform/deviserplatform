import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-admin-form',
  templateUrl: './admin-form.component.html',
  styleUrls: ['./admin-form.component.scss']
})
export class AdminFormComponent implements OnInit {
  
  metaInfo: any;
  record: any;
  constructor( private route: ActivatedRoute,
    private adminService: AdminService,
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

}
