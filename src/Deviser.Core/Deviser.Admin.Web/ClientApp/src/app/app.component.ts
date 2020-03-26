import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { DAConfig } from './common/domain-types/da-config';
import { WINDOW } from './common/services/window.service';
import { AdminConfigType } from './common/domain-types/admin-confit-type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';

  private daConfig: DAConfig;
  constructor(private router: Router,
    @Inject(WINDOW) private window: any) {
    this.daConfig = window.daConfig;
  }
  ngOnInit(): void {
    if (this.daConfig.adminConfigType === AdminConfigType.GridOnly || this.daConfig.adminConfigType === AdminConfigType.GridAndForm) {
      this.router.navigateByUrl('list')
    }
    else if (this.daConfig.adminConfigType === AdminConfigType.FormOnly) {
      this.router.navigateByUrl('detail/');
    }
  }
}
