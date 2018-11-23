import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';


import { AdminService } from '../common/services/admin.service';
import { Pagination } from '../common/domain-types/pagination'

@Component({
  selector: 'app-admin-grid',
  templateUrl: './admin-grid.component.html',
  styleUrls: ['./admin-grid.component.scss']
})
export class AdminGridComponent implements OnInit {

  metaInfo: any;
  entityRecords : any;
  pagination: Pagination;
  
  
  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.getMetaInfo();
    this.getAllRecords();   
  }


  getMetaInfo(): void {
    this.adminService.getMetaInfo('Blog', 'Post')
      .subscribe(metaInfo => this.metaInfo = metaInfo);
  }

  getAllRecords(pagination:Pagination = null):void {
    this.adminService.getAllRecords('Blog', 'Post', pagination)
    .subscribe(entityRecords => this.onGetAllRecords(entityRecords));
  }

  onChangePage(event:any):void{
    if(event.page!=null){
      this.pagination.pageNo = event.page;
    }    
    this.getAllRecords(this.pagination);
  }

  onGetAllRecords(entityRecords:any):void{
    this.entityRecords = entityRecords;
    let paging = this.entityRecords.paging;
    this.pagination = new Pagination(paging.pageNo,paging.pageSize, paging.pageCount, paging.totalRecordCount);
  }


}
