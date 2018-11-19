import { Component, OnInit } from '@angular/core';

import { AdminService } from '../services/admin.service';

@Component({
  selector: 'app-admin-grid',
  templateUrl: './admin-grid.component.html',
  styleUrls: ['./admin-grid.component.scss']
})
export class AdminGridComponent implements OnInit {

  metaInfo: any;
  entityRecords : any;

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getMetaInfo();
    this.getAllRecords();
  }


  getMetaInfo(): void {
    this.adminService.getMetaInfo('Blog', 'Post')
      .subscribe(metaInfo => this.metaInfo = metaInfo);
  }

  getAllRecords():void {
    this.adminService.getAllRecords('Blog', 'Post')
    .subscribe(entityRecords => this.entityRecords = entityRecords);
  }
}
